let siteProducts = [];   // Danh sách sản phẩm từ API
let cart = [];           // Giỏ hàng

const productList = document.getElementById("productList");
const cartItems = document.getElementById("cartItems");
const total = document.getElementById("total");

// ===================== LOAD PRODUCTS =====================
async function loadProducts() {
    try {
        const res = await fetch('/Shop/GetProducts');
        if (!res.ok) throw new Error("Không thể fetch sản phẩm");

        siteProducts = await res.json();
        renderProducts(siteProducts);
    } catch (err) {
        console.error("Lỗi load sản phẩm:", err);
        if (productList) {
            productList.innerHTML = "<p class='text-danger'>Không thể load sản phẩm</p>";
        }
    }
}

function renderProducts(list) {
    if (!productList) return;

    productList.innerHTML = "";
    list.forEach(p => {
        const col = document.createElement("div");
        col.className = "col-md-3";
        col.innerHTML = `
            <div class="card mb-3 h-100">
                <img src="/images/${p.hinhAnh}" alt="${p.tenSp}" 
                     class="card-img-top" style="height:180px; object-fit:cover;" />
                <div class="card-body text-center">
                    <h5 class="card-title">${p.tenSp}</h5>
                    <p class="card-text text-danger fw-bold">${p.donGia.toLocaleString()}đ</p>
                    <button class="btn btn-sm btn-primary" 
                        onclick="addToCart(${p.maSp}, '${p.tenSp.replace(/'/g, "\\'")}', ${p.donGia})">
                        <i class="bi bi-cart-plus"></i> Thêm giỏ
                    </button>
                </div>
            </div>
        `;
        productList.appendChild(col);
    });
}

// ===================== CART =====================
function addToCart(id, name, price) {
    const item = cart.find(x => x.id === id);
    if (item) {
        item.qty++;
    } else {
        cart.push({ id, name, price, qty: 1 });
    }
    updateCart();

    const cartCanvasEl = document.getElementById('cartOffcanvas');
    if (cartCanvasEl) {
        const cartOffcanvas = bootstrap.Offcanvas.getOrCreateInstance(cartCanvasEl);
        cartOffcanvas.show();
    }
}

function updateCart() {
    if (!cartItems || !total) return;

    cartItems.innerHTML = "";
    let sum = 0;

    if (cart.length === 0) {
        cartItems.innerHTML = "<li class='list-group-item text-center'>Giỏ hàng trống</li>";
    } else {
        cart.forEach((item, i) => {
            const lineTotal = item.price * item.qty;
            sum += lineTotal;
            const li = document.createElement("li");
            li.className = "list-group-item d-flex justify-content-between align-items-center";
            li.innerHTML = `
                <span>${item.name} x${item.qty}</span>
                <div>
                    <span class="me-2 text-danger fw-bold">${lineTotal.toLocaleString()}đ</span>
                    <button class="btn btn-sm btn-outline-danger" onclick="removeItem(${i})">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            `;
            cartItems.appendChild(li);
        });
    }

    total.innerText = `Tổng: ${sum.toLocaleString()}đ`;
}

function removeItem(index) {
    if (index >= 0 && index < cart.length) {
        cart.splice(index, 1);
        updateCart();
    }
}

// ===================== CHECKOUT =====================
function checkout() {
    if (cart.length === 0) {
        alert("Giỏ hàng trống!");
        return;
    }
    alert("Thanh toán chưa code 😁");
}

// ===================== LOGIN =====================
async function doLogin(e) {
    e.preventDefault();
    const email = document.getElementById("username")?.value.trim();
    const password = document.getElementById("password")?.value.trim();

    if (!email || !password) {
        alert("Vui lòng nhập đầy đủ Email và Mật khẩu!");
        return;
    }

    try {
        const res = await fetch('/CustomerAccount/Login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `email=${encodeURIComponent(email)}&matKhau=${encodeURIComponent(password)}`
        });

        if (!res.ok) throw new Error("Lỗi server");

        const result = await res.json();
        if (result.success) {
            alert("Đăng nhập thành công!");
            const loginModalEl = document.getElementById("loginModal");
            if (loginModalEl) {
                const loginModal = bootstrap.Modal.getInstance(loginModalEl)
                    || new bootstrap.Modal(loginModalEl);
                loginModal.hide();
            }
        } else {
            alert(result.message || "Sai email hoặc mật khẩu!");
        }
    } catch (err) {
        console.error("Lỗi login:", err);
        alert("Không thể kết nối đến server!");
    }
}

// ===================== INIT =====================
document.addEventListener("DOMContentLoaded", loadProducts);

// ===================== SEARCH =====================
function searchProduct() {
    const inputEl = document.getElementById('searchInput');
    const query = inputEl ? inputEl.value.trim() : "";

    if (!query) {
        alert("Vui lòng nhập tên sản phẩm để tìm kiếm!");
        return;
    }

    window.location.href = `/Shop/Search?query=${encodeURIComponent(query)}`;
}
