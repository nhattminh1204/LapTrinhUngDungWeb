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
    pageIndex = pIndex
    onLoad
}

//function onLoad() {
//    $("#products").load("Product/LoadProduct?idCategory=" + idCategory
//                                        + "&keyword=" + keyword
//                                        + "&pageIndex=" + pageIndex)
//}

function onLoad() {
    $("#products").load(`/Product/LoadProduct?idCategory=${idCategory}&keyword=${keyword}&pageIndex=${pageIndex}`)
}