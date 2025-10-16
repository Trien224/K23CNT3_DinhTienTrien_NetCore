// Danh sách sản phẩm giả lập
const products = [
    { id: 1, name: "Tôm hùm Alaska", category: "Hải Sản", img: "https://c.pxhere.com/photos/98/a1/leaf_green_line_vein-1286313.jpg!d", price: 500000 },
    { id: 2, name: "Gia vị lẩu Thái", category: "Gia Vị", img: "https://c.pxhere.com/photos/98/a1/leaf_green_line_vein-1286313.jpg!d", price: 50000 },
    { id: 3, name: "Son môi đỏ", category: "Mỹ Phẩm", img: "https://c.pxhere.com/photos/98/a1/leaf_green_line_vein-1286313.jpg!d", price: 200000 },
    { id: 4, name: "Áo thun Unisex", category: "Quần Áo", img: "https://c.pxhere.com/photos/98/a1/leaf_green_line_vein-1286313.jpg!d", price: 120000 },
    { id: 5, name: "Mì cay Hàn Quốc", category: "Gia Vị", img: "https://c.pxhere.com/photos/98/a1/leaf_green_line_vein-1286313.jpg!d", price: 40000 },
    { id: 6, name: "Cua Hoàng Đế", category: "Hải Sản", img: "https://c.pxhere.com/photos/98/a1/leaf_green_line_vein-1286313.jpg!d", price: 800000 },
    { id: 7, name: "Sashimi Cá Hồi", category: "Hải Sản", img: "https://c.pxhere.com/photos/98/a1/leaf_green_line_vein-1286313.jpg!d", price: 300000 },
    { id: 8, name: "Muối Ớt Tây Ninh", category: "Gia Vị", img: "https://c.pxhere.com/photos/98/a1/leaf_green_line_vein-1286313.jpg!d", price: 25000 },
    { id: 7, name: "Sashimi Cá Hồi", category: "Hải Sản", img: "https://c.pxhere.com/photos/98/a1/leaf_green_line_vein-1286313.jpg!d", price: 300000 },
    { id: 8, name: "Muối Ớt Tây Ninh", category: "Gia Vị", img: "https://c.pxhere.com/photos/98/a1/leaf_green_line_vein-1286313.jpg!d", price: 25000 },
];


let productList = document.getElementById("productList");
let cartItems = document.getElementById("cartItems");
let cartCount = document.getElementById("cartCount");
let total = document.getElementById("total");
let cart = [];

// Hiển thị sản phẩm
function renderProducts(list) {
    productList.innerHTML = "";
    list.forEach(p => {
        productList.innerHTML += `
          <div class="product">
            <img src="${p.img}" alt="${p.name}">
            <h3>${p.name} - ${p.price.toLocaleString()}đ</h3>
            <button onclick="addToCart(${p.id})">Thêm vào giỏ</button>
          </div>
        `;
    });
}
renderProducts(products);

// Thêm sản phẩm vào giỏ
function addToCart(id) {
    let product = products.find(p => p.id === id);
    let item = cart.find(c => c.id === id);
    if (item) {
        item.quantity++;
    } else {
        cart.push({ ...product, quantity: 1 });
    }
    updateCart();
}

// Cập nhật giỏ hàng
function updateCart() {
    cartItems.innerHTML = "";
    let sum = 0;
    cart.forEach((item, i) => {
        let lineTotal = item.price * item.quantity;
        sum += lineTotal;
        cartItems.innerHTML += `
          <li>
            ${item.name} x${item.quantity} - ${lineTotal.toLocaleString()}đ
            <button onclick="removeItem(${i})">❌</button>
          </li>
        `;
    });
    cartCount.innerText = cart.length;
    total.innerText = "Tổng: " + sum.toLocaleString() + "đ";
}

// Xóa sản phẩm trong giỏ
function removeItem(index) {
    cart.splice(index, 1);
    updateCart();
}

// Toggle giỏ hàng
function toggleCart() {
    let c = document.getElementById("cart");
    c.style.display = (c.style.display === "flex") ? "none" : "flex";
}

// Tìm kiếm sản phẩm
function searchProduct() {
    let keyword = document.getElementById("searchInput").value.toLowerCase();
    let filtered = products.filter(p => p.name.toLowerCase().includes(keyword));
    renderProducts(filtered);
}

// Lọc theo danh mục
function filterCategory(cat) {
    if (cat === "all") {
        renderProducts(products);
    } else {
        let filtered = products.filter(p => p.category === cat);
        renderProducts(filtered);
    }
}

// Sắp xếp sản phẩm
function sortProducts(type) {
    let sorted = [...products];
    if (type === "asc") sorted.sort((a, b) => a.price - b.price);
    if (type === "desc") sorted.sort((a, b) => b.price - a.price);
    renderProducts(sorted);
}

// Thanh toán
function checkout() {
    if (cart.length === 0) {
        alert("Giỏ hàng trống!");
    } else {
        alert("Thanh toán thành công " + cart.length + " sản phẩm!");
        cart = [];
        updateCart();
        toggleCart();
    }
}

// LOGIN popup
function toggleLogin() {
    let popup = document.getElementById("loginPopup");
    popup.style.display = (popup.style.display === "flex") ? "none" : "flex";
}

function doLogin(e) {
    e.preventDefault();
    let user = document.getElementById("username").value;
    alert("Xin chào " + user + "!");
    toggleLogin();
}
