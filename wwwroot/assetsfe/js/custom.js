jQuery(document).ready(function($) {
    jQuery('.stellarnav').stellarNav({
        theme: 'dark',
        breakpoint: 960,
        position: 'right',
        phoneBtn: '18009997788',
        locationBtn: 'https://www.google.com/maps'
    });
});

// member partner slider 
$(document).ready(function() {
  var owl = $('.fullslide.owl-carousel');
  owl.owlCarousel({    
    loop: true,
    margin: 0,
    autoplay: true,
    autoplayTimeout: 1000,
    autoplayHoverPause: true,
    responsive: {
      0: {
      items: 1
      },
       600: {
      items: 1
      },
      1000: {
      items: 2
       },
       1280: {
        items: 1
         }
   }
  });
})

// end  

// member partner slider 
$(document).ready(function() {
  var owl = $('.whatwedo.owl-carousel');
  owl.owlCarousel({    
    loop: true,
    margin: 10,
    autoplay: true,
    autoplayTimeout: 1000,
    autoplayHoverPause: true,
    responsive: {
      0: {
      items: 1
      },
       600: {
      items: 1
      },
      1000: {
      items: 2
       },
       1280: {
        items: 3
         }
   }
  });
})

// end  

// member partner slider 
$(document).ready(function() {
  var owl = $('.latestnews.owl-carousel');
  owl.owlCarousel({    
    loop: true,
    dots:false,
    margin: 10,
    autoplay: true,
    autoplayTimeout: 1000,
    autoplayHoverPause: true,
    responsive: {
      0: {
      items: 1
      },
       600: {
      items: 1
      },
      1000: {
      items: 2
       },
       1280: {
        items: 3
         }
   }
  });
})

// end  
$('.showcase').lightcase();
