let element = +document.querySelector("#Counts").innerHTML;
let button = document.getElementById('Plus');
button.addEventListener('click', function(){

    ++element;
    document.querySelector("#Counts").innerHTML=element;
})
