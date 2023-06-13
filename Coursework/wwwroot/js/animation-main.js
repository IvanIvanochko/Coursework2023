window.addEventListener('scroll', function() {
    var scrollPos = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop;
    var container = document.querySelector('.containerImg');
    var containerHeight = container.offsetHeight - 200;
    
    // Calculate the scroll percentage
    var scrollPercent = (scrollPos / containerHeight) * 100;
    
    // Calculate the updated border radius value based on the scroll percentage
    var updatedBorderRadius = '0% 0% ' + (50 - scrollPercent / 2) + '% ' + (50 - scrollPercent / 2) + '%';
    
    // Update the border radius of the image
    container.style.borderRadius = updatedBorderRadius;
  });

const observer = new IntersectionObserver((entries) => {
    entries.forEach((entry) => {
        if(entry.isIntersecting){
            entry.target.classList.add('show');
        }
        else{
            entry.target.classList.remove('show');
        }
    });
});

const hiddenElements = document.querySelectorAll('.hidden');
hiddenElements.forEach((el) => observer.observe(el));

