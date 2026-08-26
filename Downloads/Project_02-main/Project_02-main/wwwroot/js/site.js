// ===================== GLOBAL STATE & STORAGE =====================
let cart = [];
let appliedVoucher = null;

try {
    const savedCart = localStorage.getItem("shop_cart");
    if (savedCart) {
        cart = JSON.parse(savedCart);
    }
} catch (e) {
    cart = [];
}

// ===================== CART FUNCTIONS =====================
function saveCart() {
    localStorage.setItem("shop_cart", JSON.stringify(cart));
}

function addToCart(id, name, price, img = "", qty = 1) {
    id = parseInt(id);
    qty = parseInt(qty) || 1;
    price = parseFloat(price) || 0;

    const existingIndex = cart.findIndex(item => item.id === id);
    if (existingIndex > -1) {
        cart[existingIndex].qty += qty;
    } else {
        cart.push({
            id: id,
            name: name,
            price: price,
            img: img,
            qty: qty
        });
    }

    saveCart();
    updateCartUI();

    // Hiển thị offcanvas giỏ hàng
    const cartCanvasEl = document.getElementById('cartOffcanvas');
    if (cartCanvasEl) {
        const offcanvas = bootstrap.Offcanvas.getOrCreateInstance(cartCanvasEl);
        offcanvas.show();
    }
}

function changeQty(index, delta) {
    if (index >= 0 && index < cart.length) {
        cart[index].qty += delta;
        if (cart[index].qty <= 0) {
            cart.splice(index, 1);
        }
        saveCart();
        updateCartUI();
    }
}

function removeItem(index) {
    if (index >= 0 && index < cart.length) {
        cart.splice(index, 1);
        saveCart();
        updateCartUI();
    }
}

function clearCart() {
    cart = [];
    appliedVoucher = null;
    saveCart();
    updateCartUI();
}

function updateCartUI() {
    const cartItemsEl = document.getElementById("cartItems");
    const cartCountEl = document.getElementById("cartCount");
    const subTotalEl = document.getElementById("subTotal");
    const discountRowEl = document.getElementById("discountRow");
    const discountAmountEl = document.getElementById("discountAmount");
    const totalEl = document.getElementById("total");

    // 1. Cập nhật số lượng sản phẩm trên icon giỏ hàng
    const totalCount = cart.reduce((sum, item) => sum + item.qty, 0);
    if (cartCountEl) {
        cartCountEl.innerText = totalCount;
    }

    // 2. Tính tổng tiền
    let subTotal = 0;
    if (cartItemsEl) {
        cartItemsEl.innerHTML = "";

        if (cart.length === 0) {
            cartItemsEl.innerHTML = `
                <li class="list-group-item text-center py-4 text-muted">
                    <i class="bi bi-cart-x fs-1 text-secondary mb-2 d-block"></i>
                    Giỏ hàng của bạn đang trống
                </li>
            `;
            appliedVoucher = null;
            const voucherMsgEl = document.getElementById("voucherMessage");
            if (voucherMsgEl) voucherMsgEl.innerHTML = "";
            const voucherInput = document.getElementById("voucherCodeInput");
            if (voucherInput) voucherInput.value = "";
        } else {
            cart.forEach((item, index) => {
                const itemTotal = item.price * item.qty;
                subTotal += itemTotal;

                const imgSrc = item.img ? (item.img.startsWith('/') ? item.img : `/images/sanpham/${item.img}`) : '/images/no-image.png';

                const li = document.createElement("li");
                li.className = "list-group-item py-3 px-2";
                li.innerHTML = `
                    <div class="d-flex align-items-center">
                        <img src="${imgSrc}" alt="${item.name}" 
                             class="rounded me-3 border" style="width: 55px; height: 55px; object-fit: cover;"
                             onerror="this.src='/images/no-image.png'">
                        <div class="flex-grow-1">
                            <h6 class="mb-1 text-truncate" style="max-width: 170px;" title="${item.name}">${item.name}</h6>
                            <div class="text-danger fw-semibold small mb-2">${item.price.toLocaleString('vi-VN')} ₫</div>
                            <div class="d-flex align-items-center">
                                <div class="btn-group btn-group-sm" role="group">
                                    <button type="button" class="btn btn-outline-secondary px-2" onclick="changeQty(${index}, -1)">-</button>
                                    <span class="btn btn-light px-3 fw-bold disabled" style="opacity: 1;">${item.qty}</span>
                                    <button type="button" class="btn btn-outline-secondary px-2" onclick="changeQty(${index}, 1)">+</button>
                                </div>
                                <span class="ms-auto fw-bold text-dark small">${itemTotal.toLocaleString('vi-VN')} ₫</span>
                            </div>
                        </div>
                        <button class="btn btn-link text-danger ms-2 p-0" onclick="removeItem(${index})" title="Xóa">
                            <i class="bi bi-trash fs-5"></i>
                        </button>
                    </div>
                `;
                cartItemsEl.appendChild(li);
            });
        }
    } else {
        subTotal = cart.reduce((sum, item) => sum + (item.price * item.qty), 0);
    }

    // 3. Tính tiền giảm giá voucher (nếu có)
    let discount = 0;
    if (appliedVoucher && appliedVoucher.discount > 0) {
        discount = appliedVoucher.discount;
        if (discount > subTotal) discount = subTotal;
    }

    const finalTotal = Math.max(0, subTotal - discount);

    if (subTotalEl) subTotalEl.innerText = subTotal.toLocaleString('vi-VN') + " ₫";
    if (discountRowEl) {
        if (discount > 0) {
            discountRowEl.style.setProperty('display', 'flex', 'important');
            if (discountAmountEl) discountAmountEl.innerText = `-${discount.toLocaleString('vi-VN')} ₫`;
        } else {
            discountRowEl.style.setProperty('display', 'none', 'important');
        }
    }
    if (totalEl) totalEl.innerText = finalTotal.toLocaleString('vi-VN') + " ₫";
}

// ===================== VOUCHER =====================
async function applyVoucher() {
    const voucherInput = document.getElementById("voucherCodeInput");
    const voucherMsg = document.getElementById("voucherMessage");
    const code = voucherInput ? voucherInput.value.trim() : "";

    if (!code) {
        if (voucherMsg) {
            voucherMsg.className = "small mb-2 text-danger";
            voucherMsg.innerText = "Vui lòng nhập mã giảm giá!";
        }
        return;
    }

    const subTotal = cart.reduce((sum, item) => sum + (item.price * item.qty), 0);
    if (subTotal <= 0) {
        if (voucherMsg) {
            voucherMsg.className = "small mb-2 text-danger";
            voucherMsg.innerText = "Giỏ hàng đang trống!";
        }
        return;
    }

    try {
        const response = await fetch(`/Customer/HoaDons/TinhVoucher?maVoucher=${encodeURIComponent(code)}&tongTien=${subTotal}`);
        const result = await response.json();

        if (result.success) {
            appliedVoucher = {
                code: code,
                discount: result.tienGiam,
                voucherName: result.voucherName || code
            };
            if (voucherMsg) {
                voucherMsg.className = "small mb-2 text-success fw-bold";
                voucherMsg.innerText = `✓ Áp dụng thành công: Giảm ${result.tienGiam.toLocaleString('vi-VN')} ₫`;
            }
            updateCartUI();
        } else {
            appliedVoucher = null;
            if (voucherMsg) {
                voucherMsg.className = "small mb-2 text-danger";
                voucherMsg.innerText = result.message || "Mã giảm giá không hợp lệ!";
            }
            updateCartUI();
        }
    } catch (e) {
        console.error("Lỗi áp dụng voucher:", e);
        if (voucherMsg) {
            voucherMsg.className = "small mb-2 text-danger";
            voucherMsg.innerText = "Không thể kiểm tra mã giảm giá lúc này!";
        }
    }
}

// ===================== CHECKOUT MODAL & ORDER =====================
function openCheckoutModal() {
    if (cart.length === 0) {
        alert("Giỏ hàng của bạn đang trống! Hãy thêm sản phẩm trước khi thanh toán.");
        return;
    }

    // Đóng Offcanvas Cart
    const cartCanvasEl = document.getElementById('cartOffcanvas');
    if (cartCanvasEl) {
        const offcanvas = bootstrap.Offcanvas.getInstance(cartCanvasEl);
        if (offcanvas) offcanvas.hide();
    }

    // Cập nhật giá trị vào Checkout Modal
    const subTotal = cart.reduce((sum, item) => sum + (item.price * item.qty), 0);
    let discount = (appliedVoucher && appliedVoucher.discount) ? appliedVoucher.discount : 0;
    if (discount > subTotal) discount = subTotal;
    const finalTotal = Math.max(0, subTotal - discount);

    const modalSubTotalEl = document.getElementById("modalSubTotal");
    const modalDiscountRowEl = document.getElementById("modalDiscountRow");
    const modalDiscountAmountEl = document.getElementById("modalDiscountAmount");
    const modalTotalEl = document.getElementById("modalTotal");
    const alertEl = document.getElementById("checkoutAlert");

    if (modalSubTotalEl) modalSubTotalEl.innerText = subTotal.toLocaleString('vi-VN') + " ₫";
    if (modalDiscountRowEl) {
        if (discount > 0) {
            modalDiscountRowEl.style.setProperty('display', 'flex', 'important');
            if (modalDiscountAmountEl) modalDiscountAmountEl.innerText = `-${discount.toLocaleString('vi-VN')} ₫`;
        } else {
            modalDiscountRowEl.style.setProperty('display', 'none', 'important');
        }
    }
    if (modalTotalEl) modalTotalEl.innerText = finalTotal.toLocaleString('vi-VN') + " ₫";
    if (alertEl) alertEl.className = "alert d-none";

    // Mở Checkout Modal
    const modalEl = document.getElementById("checkoutModal");
    if (modalEl) {
        const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
        modal.show();
    }
}

async function submitOrder() {
    const name = document.getElementById("orderCustomerName")?.value.trim();
    const phone = document.getElementById("orderPhone")?.value.trim();
    const address = document.getElementById("orderAddress")?.value.trim();
    const note = document.getElementById("orderNote")?.value.trim();
    const alertEl = document.getElementById("checkoutAlert");
    const submitBtn = document.getElementById("btnSubmitOrder");

    if (!name || !phone || !address) {
        if (alertEl) {
            alertEl.className = "alert alert-danger";
            alertEl.innerText = "Vui lòng điền đầy đủ các thông tin bắt buộc (*)!";
        }
        return;
    }

    const payload = {
        Items: cart.map(item => ({
            ProductId: item.id,
            Quantity: item.qty
        })),
        HoTenNguoiNhan: name,
        SdtgiaoHang: phone,
        DiaChiGiaoHang: address,
        GhiChu: note,
        MaVoucher: appliedVoucher ? appliedVoucher.code : null
    };

    if (submitBtn) {
        submitBtn.disabled = true;
        submitBtn.innerHTML = `<span class="spinner-border spinner-border-sm me-2"></span> Đang xử lý đặt hàng...`;
    }

    try {
        const response = await fetch("/Customer/HoaDons/CreateOrderAjax", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(payload)
        });

        const result = await response.json();

        if (result.success) {
            if (alertEl) {
                alertEl.className = "alert alert-success";
                alertEl.innerHTML = `<strong>🎉 ${result.message}</strong><br>Cảm ơn quý khách đã tin tưởng mua sắm!`;
            }

            // Xóa giỏ hàng sau khi đặt thành công
            clearCart();

            // Tự động đóng modal sau 2.5s
            setTimeout(() => {
                const modalEl = document.getElementById("checkoutModal");
                if (modalEl) {
                    const modal = bootstrap.Modal.getInstance(modalEl);
                    if (modal) modal.hide();
                }
                const checkoutForm = document.getElementById("checkoutForm");
                if (checkoutForm) checkoutForm.reset();
                if (submitBtn) {
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = `<i class="bi bi-check2-circle me-1"></i> Xác nhận đặt hàng`;
                }
            }, 2500);
        } else {
            if (alertEl) {
                alertEl.className = "alert alert-danger";
                alertEl.innerText = result.message || "Đặt hàng không thành công!";
            }
            if (submitBtn) {
                submitBtn.disabled = false;
                submitBtn.innerHTML = `<i class="bi bi-check2-circle me-1"></i> Xác nhận đặt hàng`;
            }
        }
    } catch (e) {
        console.error("Lỗi đặt hàng:", e);
        if (alertEl) {
            alertEl.className = "alert alert-danger";
            alertEl.innerText = "Có lỗi xảy ra khi kết nối máy chủ!";
        }
        if (submitBtn) {
            submitBtn.disabled = false;
            submitBtn.innerHTML = `<i class="bi bi-check2-circle me-1"></i> Xác nhận đặt hàng`;
        }
    }
}

// ===================== SEARCH & FILTER =====================
function searchProduct() {
    const inputEl = document.getElementById('searchInput');
    const query = inputEl ? inputEl.value.trim().toLowerCase() : "";

    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        const titleEl = card.querySelector('.card-title');
        if (titleEl) {
            const title = titleEl.innerText.toLowerCase();
            const parentCol = card.closest('.col-md-3') || card.parentElement;
            if (title.includes(query)) {
                parentCol.style.display = "";
            } else {
                parentCol.style.display = "none";
            }
        }
    });
}

function filterCategory(category) {
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        const parentCol = card.closest('.col-md-3') || card.parentElement;
        if (category === 'all') {
            parentCol.style.display = "";
        } else {
            const text = card.innerText.toLowerCase();
            if (text.includes(category.toLowerCase())) {
                parentCol.style.display = "";
            } else {
                parentCol.style.display = "none";
            }
        }
    });
}

function sortProducts(order) {
    const container = document.querySelector('.container .row');
    if (!container) return;

    const cols = Array.from(container.children);
    cols.sort((a, b) => {
        const priceAEl = a.querySelector('.text-danger');
        const priceBEl = b.querySelector('.text-danger');
        const priceA = priceAEl ? parseInt(priceAEl.innerText.replace(/[^0-9]/g, '')) || 0 : 0;
        const priceB = priceBEl ? parseInt(priceBEl.innerText.replace(/[^0-9]/g, '')) || 0 : 0;

        if (order === 'asc') return priceA - priceB;
        if (order === 'desc') return priceB - priceA;
        return 0;
    });

    cols.forEach(col => container.appendChild(col));
}

// ===================== INITIALIZE =====================
document.addEventListener("DOMContentLoaded", () => {
    updateCartUI();
});
