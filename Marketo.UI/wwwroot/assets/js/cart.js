// add to cart + and -

var QtyInput = (function () {
    var $qtyInputs = $(".qty-input");

    if (!$qtyInputs.length) {
        return;
    }
   /* console.log(QtyInput);*/

    var $inputs = $qtyInputs.find(".product-qty");
    var $countBtn = $qtyInputs.find(".qty-count");
    var qtyMin = parseInt($inputs.attr("min"));
    var qtyMax = parseInt($inputs.attr("max"));

    $countBtn.click(function () {
        var operator = this.dataset.action;
        var $this = $(this);
        var $input = $this.siblings(".product-qty");
        var qty = parseInt($input.val());

        if (operator == "add") {
            qty += 1;
            if (qty >= qtyMin + 1) {
                $this.siblings(".qty-count--minus").attr("disabled", false);
            }

            if (qty >= qtyMax) {
                $this.attr("disabled", true);
            }
        } else {
            qty = qty <= qtyMin ? qtyMin : (qty -= 1);
            
            if (qty == qtyMin) {
                $this.attr("disabled", true);
            }

            if (qty < qtyMax) {
                $this.siblings(".qty-count--add").attr("disabled", false);
            }
        }

        $input.val(qty);
    });
})();

// custom add to cart add

// var valueinput=document.getElementById('value'); 
// count=1;
// function plus(){
//   count++;
//   valueinput.innerHTML=count;
// }
// function minus(){
//   if(count==1){
//     alert("minimum reqem 1 olmailidir");
//    }
//   if(count>1){
//     count--;
//     valueinput.innerHTML=count;
//   }
// }
