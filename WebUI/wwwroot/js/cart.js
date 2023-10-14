$(document).ready(function () {
    var updatedCart = []; 

    function updateCart(itemId, increment) {
        var countElement = $('.count[data-itemid="' + itemId + '"]');
        var priceElement = $('.price[data-itemid="' + itemId + '"]');
        var totalElement = $('#order-total');

        var count = parseInt(countElement.text());
        var price = parseFloat(priceElement.text());
        var total = parseFloat(totalElement.text());

        if (increment) {
            count++;
            total += price;
        } else if (count > 1) {
            count--;
            total -= price;
        }

        countElement.text(count);
        totalElement.text(total.toFixed(2));

        var existingItem = updatedCart.find(item => item.cartId === itemId);

        if (existingItem) {
            existingItem.newCount = count;
        } else {
            updatedCart.push({ cartId: itemId, newCount: count });
        }
    }
     
    $('.btn-plus, .btn-minus').on('click', function () {
        var itemId = $(this).data('itemid');
        var increment = $(this).hasClass('btn-plus');
        updateCart(itemId, increment);
    });
     
    $('#summary-button, #shop-button').on('click', function () {
        var redirectTo = $(this).attr('id').replace('-button', '');
        $.ajax({
            url: '/Cart/UpdateProductCount',
            type: 'POST',
            data: { updatedCarts: updatedCart, redirectTo: redirectTo },
            success: function (result) {
                window.location.href = result.redirectTo;
            },
            error: function (error) {
                console.log(error);
            }
        });
    });
});
