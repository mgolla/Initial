//MONTH Rooster
//function placeAfter($block) {
//    $block.after($('.Rooseter_Detail_Monthly'));
//}
//var $chosen = null;


$("#menu-toggle").click(function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});
$("#menu-toggle-2").click(function (e) {
    e.preventDefault();

    $("#wrapper").toggleClass("toggled-2");
    $('#menu ul').hide();
});

function initMenu() {
    $('#menu ul').hide();
    $('#menu ul').children('.current').parent().show();
    //$('#menu ul:first').show();
    $('#menu li a').click(
        function () {
            var checkElement = $(this).next();
            if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
                return false;
            }
            if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
                $('#menu ul:visible').slideUp('normal');
                checkElement.slideDown('normal');
                return false;
            }
        }
        );
}
$(document).ready(function () {
    resizeDiv();
    initMenu();
   
    var newHeight = $('#wrapperr').innerHeight();

    // $("#page-content-wrapper").height($("#wrapper").height());
   // alert(newHeight);

    vpw1 = $(window).width();

    if (vpw1 > 769) {

        $('.rosterList').css({ 'height': 'auto' });

        var wrapHeight = $("#wrapper").outerHeight();
       //$('#page-content-wrapper').css({ 'height': wrapHeight + 'px' });

        // alert(wrapHeight);
    }




    //    var dt = new Date();
    //    var utcDate = dt.toUTCString();
    //    document.getElementById("timestamp").innerHTML = utcDate;

    $('ul.roster-List li:nth-child(odd)').addClass('alternate');

    $(".profile-tip").click(function (event) {
        //alert("profile");
    });

    $(".rooster-day").click(function (event) {

        event.preventDefault();

       

        $(".roster_Data").addClass("DeviceRosterData");
        $(".roster_Data.actvDate").removeClass("actvDate");
        $(".rooster-day").removeClass("actvRosterEvent");

        $('[id^=roster_details]').hide();
        $('#back_calendar').show();

        $(".pageTitle").addClass("mobile_pageTitle");

        $(this).addClass("actvRosterEvent");
        $(this).closest("li").addClass("actvDate");

        document.getElementById('monthDate').innerHTML = '01 Oct 2015';
        //document.getElementById('WeekDate').innerHTML = '23 Oct 2015'; 

        $('[id^=roster_details]').slideDown("slow", function () {
            // Animation complete.
        });

         $(".rosterList").animate({ scrollTop: 0 }, 600);    

    });

     
       $(".closeTabContent").click(function (event) {
        
           $('[id^=roster_details]').fadeOut();
         $(".rooster-day.actvRosterEvent").removeClass("actvRosterEvent");
     });


    $(".extra_roster").click(function (event) {

        event.preventDefault();
        $(".roster_Data.actvDate").removeClass("actvDate");

        $(".roster_Data.actvDate").removeClass("actvDate");
        $(this).closest("li").addClass("actvDate");

    });

    $("#back_calendar").click(function (event) {
        $(".roster_Data").removeClass("DeviceRosterData");
        $(".rooster-day.actvRosterEvent").removeClass("actvRosterEvent");
        $("[id^=roster_details]").hide();
        $("#back_calendar").hide();
        $(".pageTitle").removeClass("mobile_pageTitle");
        document.getElementById('monthDate').innerHTML = 'OCT 2015';
        // document.getElementById('WeekDate').innerHTML = '23 OCT- 29 OCT'; 
    });


    $("#Time_change").click(function (event) {
        $("#changeTimeZone").show('slow').delay(5000).hide('slow');
    });

    $("input[name$='time']").click(function () {
        $("#changeTimeZone").hide();
    });

//    tabs+Accordion
     $('#parentHorizontalTab_alerts').easyResponsiveTabs({
        type: 'default', //Types: default, vertical, accordion
        width: 'auto', //auto or any width like 600px
        fit: true, // 100% fit in a container
        tabidentify: 'hor_1', // The tab groups identifier
     });
      $('#parentHorizontalTab_vision').easyResponsiveTabs({
        type: 'default', //Types: default, vertical, accordion
        width: 'auto', //auto or any width like 600px
        fit: true, // 100% fit in a container
        tabidentify: 'hor_1', // The tab groups identifier
     });

     $('#statioInfo_tab').easyResponsiveTabs({
        type: 'default', //Types: default, vertical, accordion
        width: 'auto', //auto or any width like 600px
        fit: true, // 100% fit in a container
        tabidentify: 'hor_1', // The tab groups identifier
     });

      $('#hotelInfo_tab').easyResponsiveTabs({
        type: 'default', //Types: default, vertical, accordion
        width: 'auto', //auto or any width like 600px
        fit: true, // 100% fit in a container
        tabidentify: 'hor_1', // The tab groups identifier
     });

     $('#crewInfo_tab').easyResponsiveTabs({
        type: 'default', //Types: default, vertical, accordion
        width: 'auto', //auto or any width like 600px
        fit: true, // 100% fit in a container
        tabidentify: 'hor_1', // The tab groups identifier
     });


      $('#accordiontab_documents').easyResponsiveTabs({
        type: 'accordion', //Types: default, vertical, accordion
        width: 'auto', //auto or any width like 600px
        fit: true, // 100% fit in a container
        tabidentify: 'hor_1', // The tab groups identifier
     });

      $('#accordiontab_links').easyResponsiveTabs({
        type: 'accordion', //Types: default, vertical, accordion
        width: 'auto', //auto or any width like 600px
        fit: true, // 100% fit in a container
        tabidentify: 'hor_1', // The tab groups identifier
     });




    $(".tabs-menu a").click(function (event) {
        event.preventDefault();
        $(this).parent().addClass("current");
        $(this).parent().siblings().removeClass("current");
        var tab = $(this).attr("href");
        $(".tab-content").not(tab).css("display", "none");
        $(tab).fadeIn();
    });


    //hide show
    $("#myroster_show").click(function(event){
         $("#wrapper").toggleClass("toggled");

        //$('#myroster').hide();
        $('#latestUpdates').hide();
        $('#Vision_News').hide();
        $('#document_lib').hide();
        $('#useful_links').hide();

        $('#myroster').show();

    });

    $("#latestUpdates_show").click(function(event){
        $("#wrapper").toggleClass("toggled");
        $('#myroster').hide();
        $('#latestUpdates').hide();
        $('#Vision_News').hide();
        $('#document_lib').hide();
        $('#useful_links').hide();

         $('#latestUpdates').show();
    });

    $("#Vision_News_show").click(function(event){
    $("#wrapper").toggleClass("toggled");
        $('#myroster').hide();
        $('#latestUpdates').hide();
        $('#Vision_News').hide();
        $('#document_lib').hide();
        $('#useful_links').hide();

        $('#Vision_News').show();
    });
    
    $("#document_lib_show").click(function(event){
    $("#wrapper").toggleClass("toggled");
        $('#myroster').hide();
        $('#latestUpdates').hide();
        $('#Vision_News').hide();
        $('#document_lib').hide();
        $('#useful_links').hide();

        $('#document_lib').show();
    });
    
    $("#useful_links_show").click(function(event){
    $("#wrapper").toggleClass("toggled");
        $('#myroster').hide();
        $('#latestUpdates').hide();
        $('#Vision_News').hide();
        $('#document_lib').hide();
        $('#useful_links').hide();

        $('#useful_links').show();
    });

  

 $(window).scroll(function () {

        if ($(this).scrollTop() > 100) {
            $('.scrollupWindow').fadeIn();
        } else {
            $('.scrollupWindow').fadeOut();
        }
    });
     $('.scrollupWindow').click(function () {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false;
    });


   $(".rosterList").scroll(function () {

        if ($(this).scrollTop() > 100) {
            $('.scrollup').fadeIn();
        } else {
            $('.scrollup').fadeOut();
        }
    });



    $('.scrollup').click(function () {
        $(".rosterList").animate({
            scrollTop: 0
        }, 600);
        return false;
    });



});

window.onresize = function (event) {
    resizeDiv();
    //
}

window.addEventListener("orientationchange", function() {
     resizeDiv();
     setColumnHeight();
}, false);


function resizeDiv() {
    vph = $(window).height() - 200;
    vwh = $(window).height() - 90;
    var wrapHeight2 = $("#wrapper").outerHeight();


    vpw2 = $(window).width();
   


    if (vpw2 < 768) {
      
        $('.rosterList').css({ 'height': vph + 'px' });
        $('#wrapper').css({ 'height': vwh + 'px' });
    }
    
    else if (vpw2 > 769) {
        $('.rosterList').css({ 'height': 'auto' });
        $('#latestUpdates').show();
        $('#Vision_News').show();
        $('#document_lib').show();
        $('#useful_links').show();
        $('#wrapper').css({ 'height': 'auto' });
        $('#myroster').show();
    }
    //alert(vpw);
}

function setColumnHeight() {

    vpw = $(window).width();
  
    if (vpw < 769) {
         
      

        $(".rosterList").removeClass("Roster_Monthly");
        $("[id^=roster_details]").removeClass("Rooseter_Detail_Monthly");

        $(".roster-List li").removeClass("month-fix");

        $('.rosterList > ul > li').css('height', 'auto');

    }
  
    
    else if (vpw > 769) {
     
        $(".rosterList").addClass("Roster_Monthly");
        $(".roster-List li").addClass("month-fix");
        $("[id^=roster_details]").addClass("Rooseter_Detail_Monthly");

        $('.rosterList').css({ 'height': 'auto' });
        $('#wrapper').css({ 'height': 'auto' });

       


        /*--equal height monthly roster---*/

        boxes = $('.Roster_Monthly > ul > li');
        maxHeight = Math.max.apply(
             Math, boxes.map(function () {
                 return $(this).height();
             }).get());
        boxes.height(maxHeight);

    }

}


