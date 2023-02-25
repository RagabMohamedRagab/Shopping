var cart = document.getElementById("count");
var checkout = document.getElementById("checkout");
var Nodes = document.querySelectorAll('.add-to-cart');
for (var i = 0; i < Nodes.length; i++) {
    Nodes[i].addEventListener("click", function (e) {
        var $Id = document.getElementById("Id").value;
        $.ajax({
            url: "/Cart/AddToBasket?id=" + $Id,
            type: "GET",
            success: (result) => {
                if (result == "ok") {
                    AddToCart();
                } else {
                    toastr.error("Falied To Add Cart");
                }
            }
        })
    })
}
function OnLoadPage() {
    var number = localStorage.getItem("key");
    if (number) {
        cart.textContent = number;
    }
}
function AddToCart() {
    var GetCart = localStorage.getItem("key");
    GetCart = parseInt(GetCart);
    if (GetCart) {
        GetCart = GetCart + 1;
        cart.textContent = GetCart;
        localStorage.setItem("key", GetCart);
    } else {
        cart.textContent = 1;
        localStorage.setItem("key", 1);
    }
    toastr.success("Success Add To Cart");
}
OnLoadPage();


window.addEventListener("load", function () {
    var $count = document.getElementById("count");
    $.ajax({
        url: "/Cart/GetNumberCart",
        type: "GET",
        success: (result) => {
            if (result != "no") {
                $count.textContent = 0;
                $count.textContent = result;
                localStorage.setItem("key", result);
            }
        }
    });

});

function Remove(id) {
    debugger
    var $total = $("#total");
    $.ajax({
        type:"GET",
        url: "/Cart/RemoveProduct?id="+id,
        success: function (result) {
            if (result == undefined ) {
                 toastr.error("Product Id Not Valid");
            } else {
                $("#" + id).remove();
                $total.text(result.price.toFixed(2));
                checkout.textContent = result.price.toFixed(2);
                cart.textContent = result.quentity;
                localStorage.setItem("key", result.quentity);
                if ($total.text() == undefined || $total.text() == 0) {

                    window.location.reload(true);
                }
            }
        }
    });
}























/// Shopping Cart
//$(function () {
//    $(".Quantity").on("click", function () {
//        debugger
//        var model = {
//            Id: parseInt($(this).val()),
//            Quentity: parseInt($(this).next().val())
//        };
//        $.ajax({
//            type: "POST",
//            url: "/Cart/ModifiedCart",
//            data:model,
//            success: function (result) {
//                var total = $("#total");
//                total.text("");
//                total.text(result.totalprise);
//                var price = $(".movie" + $(this).next().val());
//                price.text("");
//                price.text(result.productprice);
//                console.log(result);
//            }
//        });

//    });
//});

