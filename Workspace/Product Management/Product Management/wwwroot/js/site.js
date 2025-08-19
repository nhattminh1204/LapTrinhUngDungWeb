var idCategory = 0;
var keyword = "";
var pageIndex = 1;

$(document).ready(function () {
    $("#category").on("change", function () {
        idCategory = $(this).val();
        onLoad();
    });
});

function onSearch() {
    keyword = $("#keyword").val()
    onLoad();
}

function onEnter(event) {
    if (event.key === "Enter") {
        onSearch();
    }
}
function onPaging(pIndex) {
    pageIndex = pIndex;
    onLoad();
}

function onLoad() {
    $("#products").load(`/Product/LoadProduct?idCategory=${idCategory}&keyword=${keyword}&pageIndex=${pageIndex}`)
}


function loadUpdateModal(id) {
    $.get(`/Product/GetProduct?id=${id}`, function(data) {
        $("#updateModalContainer").html(data);
        $("#updateModal").modal('show');
    }).fail(function() {
        alert('Lỗi khi tải dữ liệu sản phẩm');
    });
}

function openDeleteModal(id, name) {
    $("#deleteProductId").val(id);
    $("#deleteProductName").text(name);
}

// Category functions
var keywordCategory = "";
var pageIndexCategory = 1;

function onSearchCategory() {
    keywordCategory = $("#keywordCategory").val();
    onLoadCategory();
}

function onEnterCategory(event) {
    if (event.key === "Enter") {
        onSearchCategory();
    }
}

function onPagingCategory(pIndex) {
    pageIndexCategory = pIndex;
    onLoadCategory();
}

function onLoadCategory() {
    $("#categories").load(`/Category/LoadCategory?keyword=${keywordCategory}&pageIndex=${pageIndexCategory}`);
}

function loadUpdateModalCategory(id) {
    $.get(`/Category/GetCategory?id=${id}`, function(data) {
        $("#updateModalContainer").html(data);
        $("#updateModal").modal('show');
    }).fail(function() {
        alert('Lỗi khi tải dữ liệu danh mục');
    });
}

function openDeleteModalCategory(id, name) {
    $("#deleteCategoryId").val(id);
    $("#deleteCategoryName").text(name);
    
    // Lấy danh sách sản phẩm thuộc danh mục
    $.get(`/Category/GetCategoryProducts?id=${id}`, function(data) {
        if (data.products && data.products.length > 0) {
            $("#productsInCategory").empty();
            data.products.forEach(function(productName) {
                $("#productsInCategory").append(`<li class="list-group-item">${productName}</li>`);
            });
            $("#productsList").show();
        } else {
            $("#productsList").hide();
        }
    }).fail(function() {
        $("#productsList").hide();
    });
}