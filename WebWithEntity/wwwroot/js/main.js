let element = document.querySelector('.header');

let Positions = function(){
    pageY= window.pageYOffset;
    pageX= window.pageXOffset;

    elTop= element.getBoundingClientRect().top;
    elBot= element.getBoundingClientRect().bottom;
    let height = Math.abs(elTop-elBot);
    let winHeight = window.innerHeight;
    console.log('pagey: '+ pageY + ' pageX: '+ pageX + ' elTop: ' +elTop+ ' elBot: '+ elBot+ ' height: ' + winHeight)
}
Positions();

window.addEventListener('scroll', function() {
    pageY= window.pageYOffset;
    elTop= element.getBoundingClientRect().top;
    if(pageY == 0 && elTop==pageY){
        
        
        element.style = "background: rgba(0,0,0);";
        
    }
    else{
        
        element.style = "background: rgba(0,0,0,0.7);";
        
    }

});