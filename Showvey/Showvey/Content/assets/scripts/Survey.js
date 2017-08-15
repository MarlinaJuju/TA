var type;
$(document).ready(function () {
    $(".input-effect input").focusout(function () {
        if ($(this).val() != "") {
            $(this).addClass("has-content");
        } else {
            $(this).removeClass("has-content");
        }
    })
    $(".next-question").click(function () {
        var resultString;
        var resultArray=new Array();
        if (type == 'Short Text' || type == 'Long Text') {
            resultString=$(".effect-20").val();
        }
        else if (type == 'Radio Button' || type == 'Checkbox' || type == 'Yes/No') {
            var inputElements = document.getElementsByClassName('questionanswer');
            for (var i = 0; inputElements[i]; ++i) {
                if (inputElements[i].checked) {
                    resultArray[i] = inputElements[i].value;
                }
            }
            //for (var i = 0; $('#answer:checked')[i]; ++i) {
            //    alert($('#answer:checked')[i].val());
            //    resultArray[i] = $('#answer:checked').val()[i];
            //}
        }
        else if (type == 'Rating') {
            resultString = $("input[type=range]").val();
        }
        ClearAnimation();
        SaveAnswers(resultString,resultArray);
    })
    $(".finish-question").click(function () {
        document.location = '/Home/Index';
    });
});

var rangeSlider = function () {
    var slider = $('.range-slider'),
        range = $('.range-slider__range'),
        value = $('.range-slider__value');

    slider.each(function () {

        value.each(function () {
            var value = $(this).prev().attr('value');
            $(this).html(value);
        });

        range.on('input', function () {
            $(this).next(value).html(this.value);
        });
    });
};

rangeSlider();

var listQuestion;
var listAnimate;
var listResponse;
var index = 0;

function Initialization() {
    
    $.ajax({
        url: '/Forms/Initialization',
        type: 'GET',
        data: { id: $("body").attr('id') },
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            
            listQuestion = data;
            FillAnimation(data[index].Animates, data[index].FontColor, data[index].FontSize);
            AddQuestion(data[index]);
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}

navigator.sayswho = (function () {
    var ua = navigator.userAgent, tem,
    M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
    if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
        return 'IE ' + (tem[1] || '');
    }
    if (M[1] === 'Chrome') {
        tem = ua.match(/\b(OPR|Edge)\/(\d+)/);
        if (tem != null) return tem.slice(1).join(' ').replace('OPR', 'Opera');
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/(\d+)/i)) != null) M.splice(1, 1, tem[1]);
    return M.join(' ');
})();

var findIP = new Promise(r=> { var w = window, a = new (w.RTCPeerConnection || w.mozRTCPeerConnection || w.webkitRTCPeerConnection)({ iceServers: [] }), b = () => { }; a.createDataChannel(""); a.createOffer(c=>a.setLocalDescription(c, b, b), b); a.onicecandidate = c=> { try { c.candidate.candidate.match(/([0-9]{1,3}(\.[0-9]{1,3}){3}|[a-f0-9]{1,4}(:[a-f0-9]{1,4}){7})/g).forEach(r) } catch (e) { } } })
findIP.then(ip => document.getElementById("unique").value = ip).catch(e => console.error(e));

function SaveAnswers(answer, answers) {
    //alert("tes");
    //alert(navigator.sayswho);
    //alert($("#unique").val());
    $.ajax({
        url: '/Forms/SaveAnswers',
        type: 'GET',
        data: {
            answer:answer,
            answers: answers.join(),
            type: type,
            questionid: listQuestion[index].Id,
            ipaddress: $("#unique").val(),
            browser: navigator.sayswho
        },
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            
            if (index+1 < listQuestion.length) {
                index++;
                
                FillAnimation(listQuestion[index].Animates, listQuestion[index].FontColor, listQuestion[index].FontSize);
                AddQuestion(listQuestion[index]);
                //if (index + 1 == listQuestion.length) {
                //    $('.next-question').css('display', 'none');
                //}
                //else {
                //    $('.next-question').css('display', 'inline');
                //}
            }
            else
            {
                
                //$('.next-question').css('display', 'inline');
                SaveData();
            }
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}

function ClearAnimation(animates) {
    $(".Background").css("display", 'none');
    $(".Character").css("display", 'none');
    $('.Character').css('animation', '');
    $('.Character').css('-moz-animation', '');
    $('.Character').css('-webkit-animation', '');
    $('.Character').css('-o-animation', '');
    $(".col-3").css("display", 'none');
    $('.col-3').css('-webkit-animation', '');
    $('.col-3').css('animation', '');
    $('.col-3').css('-webkit-animation-delay', '');
    $('.col-3').css('animation-delay', '');
}

function SaveData() {
    $.ajax({
        url: '/Forms/SaveData',
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        success: function (data) {
            Back();
        },
        error: function (xhr, status, error) {
            var err = eval("(" + xhr.responseText + ")");
            alert(err.Message);
        }
    });
}

function Back() {
    document.location = '/Forms/Finish';
}

function FillAnimation(animates, color,size) {
    
    //if (index+1 == listQuestion.length) {
    //    //$('.next-question').css('display', 'none');
    //    $('.next-question').css('display', 'inline');
    //}
    //else {
    //    $('.next-question').css('display', 'inline');
    //}
    $.each(animates, function (i) {
        var imagetype;
        if (animates[i].imageType.substr(0, animates[i].imageType.indexOf(' ')) == '') {
            imagetype = animates[i].imageType;
        }
        else {
            imagetype = animates[i].imageType.substr(0, animates[i].imageType.indexOf(' '));
        }
        $("." + imagetype).attr("src", animates[i].Location);
        $("." + imagetype).css("display", 'inline');
        if (imagetype == 'Character') {
            $(".Character").css("width", animates[i].Width/1.75);
            $(".Character").css("height", animates[i].Height/1.75);
            $('.Character').css('left', (animates[i].PosX*2.4)/1.75);
            $('.Character').css('top', (animates[i].PosY * 2.4) / 1.75);
            $('.Character').css('display', 'inline');
            $('.Character').css('animation', 'fadein 2s');
            $('.Character').css('-moz-animation', 'fadein 2s');
            $('.Character').css('-webkit-animation', 'fadein 2s');
            $('.Character').css('-o-animation', 'fadein 2s');
        }
    });
    if (size > 0) {
        $('.dialog-text').css('font-size', size);
    }
    else {
        $('.dialog-text').css('font-size', 24);
    }
    $('.dialog-text').css('color', color);

}

function AddQuestion(question) {
    var text;
    if (question.Content.substr(0, question.Content.indexOf('?')) == '') {
        text = question.Content;
    }
    else {
        text = question.Content.substr(0, question.Content.indexOf('?'));
    }
    
    $('.question-text').html('<p>'+text+'<span class="ask">?</span></p>');
    type = question.Type;
    var strResult = '';
    if (question.Type == 'Short Text') {
        $('.col-3').html('<div class="input-effect"><input class="effect-20" type="text" placeholder="" onblur="cek()" onfocusout="cek()"><label><i>My answer</i></label><span class="focus-border"><i></i></span></div>');
    }
    else if (question.Type == 'Long Text') {
        $('.col-3').html('<div class="input-effect"><input class="effect-20" type="text" placeholder="" onblur="cek()" onfocusout="cek()"><label><i>My answer</i></label><span class="focus-border"><i></i></span></div>');
        $('.col-3').css('width', '75%');
    }
    else if (question.Type == 'Rating') {
        $('.col-3').html('<input class="range-slider__range" type="range" value="0" min="0" max="'+question.Count+'"><span class="range-slider__value">0</span>');
    }
    else if (question.Type == 'Checkbox') {
        var q=question.QuestionAnswers;
        $.each(q, function (i) {
            strResult += '<input type="checkbox" class="questionanswer" name="answer" value="' + q[i].Id + '"> '+q[i].Answer+'<br />';
        });
        $('.col-3').html(strResult);
    }
    else if (question.Type == 'Radio Button') {
        var q = question.QuestionAnswers;
        $.each(q,function(i){
            strResult += '<input type="radio" class="questionanswer" name="answer" value="' + q[i].Id + '"> ' + q[i].Answer + '<br />';
        });
        $('.col-3').html(strResult);
    }
    else if (question.Type == 'Yes/No') {
        $('.col-3').html('<input type="radio" name="answer" value="yes"> Yes<br /><input type="radio" name="answer" value="no"> No<br />');
    }
    $('.col-3').css('display', 'inline');
    $('.col-3').css('-webkit-animation', 'show 1s forwards');
    $('.col-3').css('animation', 'show 1s forwards');
    $('.col-3').css('-webkit-animation-delay', '4s');
    $('.col-3').css('animation-delay', '4s');
}