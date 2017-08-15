//edit question
function EditToggle(id,total) {
    $(".Edit" + id).slideToggle();
}

//show result//
function ShowToggle(id) {
    $(".Response" + id).slideToggle();
}


//choose image type
var before = "type1";

function TypeClicked(a,val) {
        document.getElementById(before).className = "hover";
    document.getElementById(a).className = "selected";
    before = a;
    CallImage(val);
}

function CallImage(val) {
    $.ajax({
        url: '/Designs/Image',
        type: 'GET',
        data: { type: val },
        contentType: 'application/json; charset=utf-8',
        dataType:"json",
        success: function (data) {
            WriteResponse(data);
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}

function AddCharacter(width, height, x, y) {
    //alert("Ini name image: "+$('.selected').attr('data-value'));
    $.ajax({
        url: '/Designs/AddCharacter',
        type: 'GET',
        data: {
            questionId: $('#QuestionId').val(),
            x: x,
            y: y,
            width: width,
            height: height
        },
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            //alert("success");
            //alert(data);
            //WriteCanvas(data);
        },
        error: function (xhr, status, error) {
            //alert("error yey!");
            //alert(selected);
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}

function AddAnimates(width, height, x, y) {
    //alert("Ini name image: "+$('.selected').attr('data-value'));
    $.ajax({
    url: '/Designs/AddAnimates',
    type: 'GET',
    data: {
        imageType: $('.selected').attr('data-value'),
        imageName:selected,
        questionId: $('#QuestionId').val(),
        x:x,
        y:y,
        width:width,
        height:height
    },
    contentType: 'application/json; charset=utf-8',
    success: function (data) {
        //alert("success");
        //alert(data);
        //WriteCanvas(data);
        $('#loader').css('display', 'none');
    },
    error: function (xhr, status, error) {
        $('#loader').css('display', 'none');
        //alert("error yey!");
        //alert(selected);
        var err = eval("(" + xhr.responseText + ")");
        alert(err.Message);
    }
});
}

function GetAnimates() {
    $.ajax({
        url: '/Designs/GetAnimates',
        type: 'GET',
        data: { questionId: $('#QuestionId').val() },
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            //alert("success");
            ShowAnimates(data);
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}

function SaveColor(color,size) {
    $.ajax({
        url: '/Designs/SaveColor',
        type: 'GET',
        data: {
            questionId: $('#QuestionId').val(),
            color: color,
            size:size
        },
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            $('#loader').css('display', 'none');
            //alert("success");
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}

function GetColor() {
    $.ajax({
        url: '/Designs/GetColor',
        type: 'GET',
        data: {
            questionId: $('#QuestionId').val()
        },
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            //alert("success");
            $('#loader').css('display', 'none');
            $('.dialog-text').css('color', data);
        },
        error: function (xhr, status, error) {
            $('#loader').css('display', 'none');
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}

function GetSize() {
    $.ajax({
        url: '/Designs/GetSize',
        type: 'GET',
        data: {
            questionId: $('#QuestionId').val()
        },
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            //alert("success");
            $('#loader').css('display', 'none');
            $('.dialog-text').css('font-size', (data*1.9)/2.4);
            $('#change-size').value = data;
        },
        error: function (xhr, status, error) {
            $('#loader').css('display', 'none');
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}

var charName;
var charType;
function ShowAnimates(data) {
    //alert("masuk");
    var strResult = "";
    var char = false;
    var location,w,h,x,y,id;
    $.each(data, function (i) {
        
        if (data[i].imageType == "Character") {
            char = true;
            location = data[i].Location;
            //alert("ini di dalam "+data[i].Location);
            w=data[i].Width;
            h=data[i].Height;
            x=data[i].PosX;
            y = data[i].PosY;
            id = data[i].ImageId;
            $('.image-control').css('display', "block");
            document.getElementById('positionX').value = data[i].PosX;
            document.getElementById('positionY').value = data[i].PosY;
            document.getElementById('char-width').value = data[i].Width/2.4;
            document.getElementById('char-height').value = data[i].Height/2.4;
            strResult += '<div><canvas id="canvas" width=800 height=450></canvas></div>';

            charName = data[i].imageId;
            charType = data[i].imageType;
        }
        else {
            //alert(data[i].Location);
            strResult += '<div><img src=' + data[i].Location + ' class=' + data[i].imageType + ' /></div>';
        }
    });
    strResult+='<div class="dialog-text">This is a sample question</div>';
    //strResult += $(".window-preview").html();
    $(".window-preview").html(strResult);
    if (char == true) {
        //alert("mulai dari sini?");
        //alert(w);
        //alert(h);
        InitializeCanvas(location, w, h, x, y,id);
    }
}

function WriteResponse(data) {
    var strResult = "<tr>";
    $.each(data, function (i) {
        if (i > 0 && i % 3 == 0) {
            strResult += '</tr><tr><td><div class="img-border"><a class="thumb-image" href="#"><img src=' + data[i].Location + ' alt=' + i + ' class="animate-image" data-value=' + data[i].Id + ' id="image"' + i + ' onclick="CekGambar(this.getAttribute('+"'data-value'"+'))"></a></div></td>';
        }
        else {
            strResult += '<td><div class="img-border"><a class="thumb-image" href="#"><img src=' + data[i].Location + ' alt=' + i + ' class="animate-image" data-value=' + data[i].Id + ' id="image"' + i + ' onclick="CekGambar(this.getAttribute(' + "'data-value'" + '))"></a></div></td>';
        }
    });
    strResult += '</tr>';
    $("#img-result").html(strResult);
}

var selected;

function CekGambar(value) {
    //alert("ini value "+value);
    selected = value;

}


//apply to all animate
function ApplyAll() {
    alert("haha");
    $.ajax({
        url: '/Designs/ApplyAll',
        type: 'GET',
        data: {
            questionId: $('#QuestionId').val(),
            color:$('.dialog-text').css('color'),
            size:$('.change-size').val()
        },
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            //alert("success");
            $('#loader').css('display', 'none');
        },
        error: function (xhr, status, error) {
            $('#loader').css('display', 'none');
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}

$(document).ready(function () {

    //add question and question answer
    $(".add-quest").click(function () {
        $(".quest").slideToggle();
    });
    var $fields = $('#fields');
    var x = 1;
    $('#btnAddField').click(function (e) {
        e.preventDefault();
        x++;
        $('<div class="form-group"><div class="col-md-2"></div><div class="col-md-5"><input type="text" name="dynamicField" id="dynamicField" class="form-control"></div><div class="col-md-1"><a href="#" class="remove_field btn btn-plus"><i class="fa fa-times"></i></a></div></div>').appendTo($fields);
    });
    $('#fields').on("click", ".remove_field", function (e) { //user click on remove text
        e.preventDefault();
        $(this).parent('div').parent('div').remove();
        x--;
    });
    //end add question//

    //edit question//
    $(".edit-quest").click(function () {
        $(".edit-question").slideToggle();
    });
    var $editfields = $('#editfields');
    var x = 1;
    $('#btnAddEditField').click(function (e) {
        e.preventDefault();
        x++;
        $('<div class="form-group"><div class="col-md-2"></div><div class="col-md-5"><input type="text" name="editdynamicField" id="dynamicField" class="form-control"></div><div class="col-md-1"><a href="#" class="editremove_field btn btn-plus"><i class="fa fa-times"></i></a></div></div>').appendTo($editfields);
    });
    $('#editfields').on("click", ".editremove_field", function (e) { //user click on remove text
        e.preventDefault();
        $(this).parent('div').parent('div').remove();
        x--;
    });
    //end edit question//

    //resize character//

    $('#positionX').change(function () {
        if ($(this).val() >= 0 && $(this).val() < 700) {
            imageX = $(this).val();
            draw(true, false);
        }
        else {
            alert("Position out of view!");
        }
    })
    $('#positionY').change(function () {
        if ($(this).val() >= 0 && $(this).val() < 450) {
            imageY = $(this).val();
            draw(true, false);
        }
        else {
            alert("Position out of view!");
        }
    })
    $('#char-height').change(function () {
        if ($(this).val() >= 0 && $(this).val() < 450) {
            imageHeight = $(this).val();
            draw(true, false);
        }
        else {
            alert("Size out of view!");
        }
    })
    $('#char-width').change(function () {
        if ($(this).val() >= 0 && $(this).val() < 700) {
            imageWidth = $(this).val();
            draw(true, false);
        }
        else {
            alert("Size out of view!");
        }
    })

    //end resize character//


    //add animation

    $('.btn-image').click(function () {
        $('#loader').css('display', 'block');
        AddAnimates(0,0,0,0);
    });

    //$('#save-changes').click(function () {
    //    alert("masuk save changes ");
    //   // AddAnimates((imageWidth * 2.4), (imageHeight * 2.4), imageX, imageY);
    //    //SaveColor($('.dialog-text').css('color'));
    //});
    $('#savechanges').click(function () {
        $('#loader').css('display', 'block');
        if (charName != null || charName != "") {
            AddCharacter((imageWidth * 2.4), (imageHeight * 2.4), imageX, imageY);
        }
        SaveColor($('.dialog-text').css('color'), $('#change-size').val());
    });
    $('#savepreview').click(function () {
        $('#loader').css('display', 'block');
        if (charName != null || charName != "") {
            AddCharacter((imageWidth * 2.4), (imageHeight * 2.4), imageX, imageY);
        }
        SaveColor($('.dialog-text').css('color'), $('#change-size').val());
    });
    $('#refresh').click(function () {
        $('#loader').css('display', 'block');
        GetAnimates();
        GetColor();
        GetSize();
    });
    $('#applyall').click(function () {
        alert("haha");
        $('#loader').css('display', 'block');
        ApplyAll();
    });
    //modal add animation//
    var modal = document.getElementById("show-animasi");
    //var btn = document.getElementById("add-animasi");
    //var span = document.getElementById("close-animation")[0];

    //open modal
    $('.add-animasi').click(function () {
        CallImage($(this).attr('data-value'));
        modal.style.display = "block";
    });

    //x close modal
    $('.close-animation').click(function () {
        modal.style.display = "none";
        GetAnimates();
        GetColor();
        GetSize();
    });

    //close by click anywhere
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
            GetAnimates();
            GetColor();
            GetSize();
        }
    }

    //font color and size change

    $('#change-color').change(function () {
        $('.dialog-text').css('color', $(this).val());
    })
    $('#change-size').change(function () {
        $('.dialog-text').css('font-size', ($(this).val()*1.9)/2.4);
    })

        //font color end size change
});
$(function () {
    // tooltip demo
    $('.tooltip-demo').tooltip({
        selector: "[data-toggle=tooltip]",
        container: "body"
    })

    // popover demo
    $("[data-toggle=popover]")
        .popover()
    ///calling side menu

    $('#side-menu').metisMenu();

    ///pace function for showing progress

    function load(time) {
        var x = new XMLHttpRequest()
        x.open('GET', "" + time, true);
        x.send();
    };

    load(20);
    load(100);
    load(500);
    load(2000);
    load(3000);
    setTimeout(function () {
        Pace.ignore(function () {
            load(3100);
        });
    }, 4000);

    Pace.on('hide', function () {
        console.log('done');
    });
    paceOptions = {
        elements: true
    };
   

});

//Loads the correct sidebar on window load, collapses the sidebar on window resize.
$(function() {
    $(window).bind("load resize", function() {
        console.log($(this).width())
        if ($(this).width() < 768) {
            $('div.sidebar-collapse').addClass('collapse')
        } else {
            $('div.sidebar-collapse').removeClass('collapse')
        }
    })
})

//resizing and dragging image

    //var canvas = document.getElementById("canvas");
    //var ctx = canvas.getContext("2d");

    //var canvasOffset = $("#canvas").offset();
    //var offsetX = canvasOffset.left;
    //var offsetY = canvasOffset.top;

    //var startX;
    //var startY;
    //var isDown = false;


    //var pi2 = Math.PI * 2;
    //var resizerRadius = 8;
    //var rr = resizerRadius * resizerRadius;
    //var draggingResizer = {
    //    x: 0,
    //    y: 0
    //};
    //var imageX = 50;
    //var imageY = 50;
    //var imageWidth, imageHeight, imageRight, imageBottom;
    //var draggingImage = false;
    //var startX;
    //var startY;



    //var img = new Image();

    //    img.onload = function () {
    //        imageWidth = (img.width / 2.4);
    //        imageHeight = (img.height / 2.4);
    //        imageRight = imageX + imageWidth;
    //        imageBottom = imageY + imageHeight
    //        draw(true, false);
    //    }
//    img.src = "/Content/assets/img/home/4.png";


var canvas;
var ctx;
var canvasOffset;
var offsetX;
var offsetY;
var startX;
var startY;
var isDown;
var pi2;
var resizerRadius;
var rr;
var draggingResizer;
var imageX;
var imageY;
var imageWidth, imageHeight, imageRight, imageBottom;
var draggingImage;
var startX;
var startY;
var gambarId;
var img = new Image();
        
        function InitializeCanvas(l,width,height,posx,posy,id) {
            canvas = document.getElementById("canvas");
            ctx = canvas.getContext("2d");

            canvasOffset = $("#canvas").offset();
            offsetX = canvasOffset.left;
            offsetY = canvasOffset.top;

            isDown = false;

            pi2 = Math.PI * 2;
            resizerRadius = 8;
            rr = resizerRadius * resizerRadius;
            draggingResizer = {
                x: 0,
                y: 0
            };
            imageX = posx;
            imageY = posy;
            draggingImage = false;

            img.onload = function () {
                imageWidth = (width / 2.4);
                imageHeight = (height / 2.4);
                imageRight = imageX + imageWidth;
                imageBottom = imageY + imageHeight
                draw(true, false);
            }
            img.src = l;
            selected = id;
        }

function draw(withAnchors, withBorders) {

    // clear the canvas
    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // draw the image
    ctx.drawImage(img, 0, 0, img.width, img.height, imageX, imageY, imageWidth, imageHeight);

    //// optionally draw the draggable anchors
    //if (withAnchors) {
    //    drawDragAnchor(imageX, imageY);
    //    drawDragAnchor(imageRight, imageY);
    //    drawDragAnchor(imageRight, imageBottom);
    //    drawDragAnchor(imageX, imageBottom);
    //}

    //// optionally draw the connecting anchor lines
    //if (withBorders) {
    //    ctx.beginPath();
    //    ctx.moveTo(imageX, imageY);
    //    ctx.lineTo(imageRight, imageY);
    //    ctx.lineTo(imageRight, imageBottom);
    //    ctx.lineTo(imageX, imageBottom);
    //    ctx.closePath();
    //    ctx.stroke();
    //}

}

function drawDragAnchor(x, y) {
    ctx.beginPath();
    ctx.arc(x, y, resizerRadius, 0, pi2, false);
    ctx.closePath();
    ctx.fill();
}

function anchorHitTest(x, y) {

    var dx, dy;

    // top-left
    dx = x - imageX;
    dy = y - imageY;
    if (dx * dx + dy * dy <= rr) {
        return (0);
    }
    // top-right
    dx = x - imageRight;
    dy = y - imageY;
    if (dx * dx + dy * dy <= rr) {
        return (1);
    }
    // bottom-right
    dx = x - imageRight;
    dy = y - imageBottom;
    if (dx * dx + dy * dy <= rr) {
        return (2);
    }
    // bottom-left
    dx = x - imageX;
    dy = y - imageBottom;
    if (dx * dx + dy * dy <= rr) {
        return (3);
    }
    return (-1);

}


function hitImage(x, y) {
    return (x > imageX && x < imageX + imageWidth && y > imageY && y < imageY + imageHeight);
}


function handleMouseDown(e) {
    startX = parseInt(e.clientX - offsetX);
    startY = parseInt(e.clientY - offsetY);
    draggingResizer = anchorHitTest(startX, startY);
    draggingImage = draggingResizer < 0 && hitImage(startX, startY);
}

function handleMouseUp(e) {
    draggingResizer = -1;
    draggingImage = false;
    draw(true, false);
}

function handleMouseOut(e) {
    handleMouseUp(e);
}

function handleMouseMove(e) {

    if (draggingResizer > -1) {

        mouseX = parseInt(e.clientX - offsetX);
        mouseY = parseInt(e.clientY - offsetY);

        // resize the image
        switch (draggingResizer) {
            case 0:
                //top-left
                imageX = mouseX;
                imageWidth = imageRight - mouseX;
                imageY = mouseY;
                imageHeight = imageBottom - mouseY;
                break;
            case 1:
                //top-right
                imageY = mouseY;
                imageWidth = mouseX - imageX;
                imageHeight = imageBottom - mouseY;
                break;
            case 2:
                //bottom-right
                imageWidth = mouseX - imageX;
                imageHeight = mouseY - imageY;
                break;
            case 3:
                //bottom-left
                imageX = mouseX;
                imageWidth = imageRight - mouseX;
                imageHeight = mouseY - imageY;
                break;
        }

        if (imageWidth < 25) { imageWidth = 25; }
        if (imageHeight < 25) { imageHeight = 25; }

        // set the image right and bottom
        imageRight = imageX + imageWidth;
        imageBottom = imageY + imageHeight;

        // redraw the image with resizing anchors
        draw(true, true);

    } else if (draggingImage) {

        imageClick = false;

        mouseX = parseInt(e.clientX - offsetX);
        mouseY = parseInt(e.clientY - offsetY);

        // move the image by the amount of the latest drag
        var dx = mouseX - startX;
        var dy = mouseY - startY;
        imageX += dx;
        imageY += dy;
        imageRight += dx;
        imageBottom += dy;
        // reset the startXY for next time
        startX = mouseX;
        startY = mouseY;

        // redraw the image with border
        draw(false, true);

    }


}


$(".window-preview").mousedown(function (e) {
    handleMouseDown(e);
});
$(".window-preview").mousemove(function (e) {
    handleMouseMove(e);
});
$(".window-preview").mouseup(function (e) {
    handleMouseUp(e);
});
$(".window-preview").mouseout(function (e) {
    handleMouseOut(e);
});