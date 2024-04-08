// AOS 

AOS.init();

// accordian AllCategories

var acc = document.getElementsByClassName("accordion");
var i;

for (i = 0; i < acc.length; i++) {
    acc[i].addEventListener("click", function() {
      this.classList.toggle("accshow");
      var dropdown = this.nextElementSibling;
      if (dropdown.style.display === "block") {
        dropdown.style.display = "none";
      } else {
        dropdown.style.display = "block";
      }
      // console.log(dropdown);
    });
  
}

// checkout click d-block

// function myFunction() {
//   document.getElementById('form-login-hidden').style.display = 'block';
// }

function myFunction() {
  let x = document.getElementById("form-login-hidden");
  if (x.style.display === "none") {
    x.style.display = "block";
  } else {
    x.style.display = "none";
  }
}
function checkEmail(email) {
    var reg = /^([a-zA-Z0-9_\-\.]+)@([a - zA - Z0 - 9_\-\.] +)\.([a-zA-Z]{2,4})$/;

    if (!reg.test(email)) return false;
    return true;
}

//SideBar menu

let menu = document.getElementById('menu');
let sidebar = document.getElementById('sidebar');
let close = document.getElementById('close')

close.addEventListener('click', function(){
    sidebar.classList.remove('menu-active')
})

menu.addEventListener('click', function(){
    sidebar.classList.add('menu-active');
})

// pageYOFFset
window.addEventListener('scroll', function(){
	let well = document.querySelector(".headerbottom");

	if(window.pageYOffset>200){
		well.classList.add('sticky');
	}else{
    well.classList.remove('sticky');  
    }
})


// Recently Added tab

let btnav =document.querySelectorAll(".navbtn");
let contentbody =document.querySelectorAll(".content_tab");
for (let btn of btnav) {
  btn.addEventListener("click",function(){
    let active =document.querySelector(".active_show");
    console.log(active);
    active.classList.remove("active_show")
    this.classList.add("active_show")
    let index=this.getAttribute("id");
    for (let div of contentbody) {
      if(index==div.getAttribute("id")){
        div.style="display:block";
      }
      else{
        div.style="display:none";
      }
      }
    })
}

// New Arrivals tab
let btnavlink=document.querySelectorAll(".btnnav");
let content =document.querySelectorAll(".arrival_content");
for (let btn of btnavlink) {
  btn.addEventListener("click",function(){
    let active =document.querySelector(".contentshow");
    console.log(active);
    active.classList.remove("contentshow")
    this.classList.add("contentshow")
    let index=this.getAttribute("id");
    for (let div of content) {
      if(index==div.getAttribute("id")){
        div.style="display:block";
      }
      else{
        div.style="display:none";
      }
      }
    })
}

// Product Details tab

let btnall=document.querySelectorAll(".btnclick");
let divs =document.querySelectorAll(".tab_body");
for (let btn of btnall) {
  btn.addEventListener("click",function(){
    let active =document.querySelector(".active");
    console.log(active);
    active.classList.remove("active")
    this.classList.add("active")
    let index=this.getAttribute("id");
    for (let div of divs) {
      if(index==div.getAttribute("id")){
        div.style="display:block";
      }
      else{
        div.style="display:none";
      }
      }
    })
}

// in shop view tab

let btnlink = document.querySelectorAll(".btn_link");
let shop_content =document.querySelectorAll(".shop_content");
for (let btn of btnlink) {
  btn.addEventListener("click",function(){
    let active =document.querySelector(".active_link");
    console.log(active);
    active.classList.remove("active_link")
    this.classList.add("active_link")
    let index=this.getAttribute("id");
    for (let div of shop_content) {
      if(index==div.getAttribute("id")){
        div.style="display:block";
      }
      else{
        div.style="display:none";
      }
      }
    })
}
// $(function () {
//   var tab = $(".nav-item"),
//   content = $(".shop_content");

//   tab.filter(":first").addClass("active");

//   content.filter(":not(:first)").hide();

//   tab.click(function () {
//       var indeks = $(this).index();
//       tab.removeClass("active").eq(indeks).addClass("active");
//       console.log(indeks);
//       content.hide().eq(indeks).fadeIn(500);
//     // console.log(content);
//       return false;
//   });
// });


// Back to top button

      //   $(window).scroll(function () {
      //     if ($(this).scrollTop() > 100) {
      //         $('.back-to-top').fadeIn('slow');
      //     } else {
      //         $('.back-to-top').fadeOut('slow');
      //     }
      // });
      // $('.back-to-top').click(function () {
      //     $('html, body').animate({scrollTop: 0}, 1500, 'easeInOutExpo');
      //     return false;
      // });
      
// account

