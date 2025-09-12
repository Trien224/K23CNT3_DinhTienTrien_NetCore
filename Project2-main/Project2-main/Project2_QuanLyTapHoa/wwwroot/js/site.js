let siteProducts = [];   // Danh sách sản phẩm từ API
let cart = [];           // Giỏ hàng

const productList = document.getElementById("productList");
const cartItems = document.getElementById("cartItems");
const total = document.getElementById("total");

// ===================== LOAD PRODUCTS =====================
async function loadProducts() {
    try {
        const res = await fetch('/Shop/GetProducts');
        siteProducts = await res.json();
        renderProducts(siteProducts);
    } catch (err) {
        console.error("Lỗi load sản phẩm:", err);
        productList.innerHTML = "<p>Không thể load sản phẩm</p>";
    }
}

function renderProducts(list) {
    productList.innerHTML = "";
    list.forEach(p => {
        const col = document.createElement("div");
        col.className = "col-md-3";
        col.innerHTML = `
            <div class="card mb-3">
              <img src="/images/${p.hinhAnh}" alt="${p.tenSp}" width="100" />
                <div class="card-body text-center">
                    <h5>${p.tenSp}</h5>
                    <p>${p.donGia.toLocaleString()}đ</p>
                    <button class="btn btn-sm btn-primary" onclick="addToCart(${p.maSp}, '${p.tenSp}', ${p.donGia})">
                        Thêm giỏ
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
    if (item) item.qty++;
    else cart.push({ id, name, price, qty: 1 });
    updateCart();
    const cartOffcanvas = new bootstrap.Offcanvas(document.getElementById('cartOffcanvas'));
    cartOffcanvas.show();
}

function updateCart() {
    cartItems.innerHTML = "";
    let sum = 0;
    if (cart.length === 0) {
        cartItems.innerHTML = "<li class='list-group-item text-center'>Giỏ hàng trống</li>";
    }
    cart.forEach((item, i) => {
        const lineTotal = item.price * item.qty;
        sum += lineTotal;
        const li = document.createElement("li");
        li.className = "list-group-item d-flex justify-content-between align-items-center";
        li.innerHTML = `
            ${item.name} x${item.qty}
            <div>
                <span class="me-2">${lineTotal.toLocaleString()}đ</span>
                <button class="btn btn-sm btn-outline-danger" onclick="removeItem(${i})">
                    <i class="bi bi-trash"></i>
                </button>
            </div>
        `;
        cartItems.appendChild(li);
    });
    total.innerText = `Tổng: ${sum.toLocaleString()}đ`;
}

function removeItem(index) {
    cart.splice(index, 1);
    updateCart();
}

// ===================== CHECKOUT =====================
function checkout() {
    if (cart.length === 0) return alert("Giỏ hàng trống!");
    alert("Thanh toán chưa code 😁");
}

// ===================== LOGIN =====================
async function doLogin(e) {
    e.preventDefault();
    const email = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    try {
        const res = await fetch('/CustomerAccount/Login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            body: `email=${encodeURIComponent(email)}&matKhau=${encodeURIComponent(password)}`
        });
        const result = await res.json();
        if (result.success) {
            alert("Đăng nhập thành công!");
            const loginModal = bootstrap.Modal.getInstance(document.getElementById("loginModal"));
            loginModal.hide();
        } else {
            alert(result.message || "Sai email hoặc mật khẩu!");
        }
    } catch (err) {
        console.error("Lỗi login:", err);
    }
}

// ===================== INIT =====================
document.addEventListener("DOMContentLoaded", loadProducts);

function searchProduct() {
    const query = document.getElementById('searchInput').value.trim();
    if (!query) {
        alert("Vui lòng nhập tên sản phẩm để tìm kiếm!");
        return;
    }
    // Chuyển hướng tới action Search trên ShopController
    window.location.href = `/Shop/Search?query=${encodeURIComponent(query)}`;
}
