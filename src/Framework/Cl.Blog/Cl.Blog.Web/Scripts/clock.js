// inner variables
var canvas, ctx;
var clockRadius;
var clockImage;


// draw functions :
function clear() { // clear canvas function
    ctx.clearRect(0, 0, ctx.canvas.width, ctx.canvas.height);
}
//绘制
function drawScene() { // main drawScene function
    clear(); // clear canvas

    // 获取当前时间：12小时制
    var date = new Date();
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var seconds = date.getSeconds();
    hours = hours > 12 ? hours - 12 : hours;
    var hour = hours + minutes / 60;
    var minute = minutes + seconds / 60;

    // save current context
    ctx.save();

    // draw clock image (as background)
    //ctx.drawImage(clockImage, 0, 0, 2 * clockRadius, 2 * clockRadius);

    //#region 画表盘圆
    ctx.translate(canvas.width / 2, canvas.height / 2);
    ctx.beginPath();
    ctx.strokeStyle = 'red';
    ctx.arc(0, 0, clockRadius, 0, 2 * Math.PI, true);
    ctx.fillStyle = "#ebf0eb";
    ctx.fill();
    ctx.restore();    
    //#endregion

    //画分钟刻度
    for (var i = 0; i < 60; i++) {
        if (i % 5 == 0) {
            continue;
        }
        ctx.save();
        var angle = i * 6 * Math.PI / 180;
        ctx.translate(canvas.width / 2, canvas.height / 2);
        ctx.beginPath();
        ctx.strokeStyle = "red";
        ctx.lineWidth = 1;
        ctx.moveTo(clockRadius * Math.cos(angle), clockRadius * Math.sin(angle));
        ctx.lineTo((clockRadius - 5) * Math.cos(angle), (clockRadius - 5) * Math.sin(angle));
        ctx.stroke();
        ctx.restore();
    }
    //画时钟刻度
    for (var i = 0; i < 12; i++) {
        ctx.save();
        var angle = i * 30 * Math.PI / 180;
        ctx.translate(canvas.width / 2, canvas.height / 2);
        ctx.beginPath();
        ctx.strokeStyle = "#0094ff";
        ctx.lineWidth = 2;
        ctx.moveTo(clockRadius * Math.cos(angle), clockRadius * Math.sin(angle));
        ctx.lineTo((clockRadius - 8) * Math.cos(angle), (clockRadius - 8) * Math.sin(angle));
        ctx.stroke();
        ctx.restore();
    }

    //#region 画1-12的数字
    ctx.save();
    ctx.translate(canvas.width / 2, canvas.height / 2);
    ctx.beginPath();
    ctx.font = '18px Arial';
    ctx.fillStyle = '#000';
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';
    for (var n = 1; n <= 12; n++) {
        var theta = (n - 3) * (Math.PI * 2) / 12;
        var x = clockRadius * 0.8 * Math.cos(theta);
        var y = clockRadius * 0.8 * Math.sin(theta);
        ctx.fillText(n, x, y);
    }
    //#endregion

    //#region 画时针    
    ctx.save();
    var theta = (hour - 3) * 2 * Math.PI / 12;
    ctx.rotate(theta);
    ctx.beginPath();
    ctx.moveTo(-15, -5);
    ctx.lineTo(-15, 5);
    ctx.lineTo(clockRadius * 0.6, 1);
    ctx.lineTo(clockRadius * 0.6, -1);
    ctx.fill();
    ctx.restore();
    //#endregion

    //#region 画分针
    ctx.save();
    var theta = (minute - 15) * 2 * Math.PI / 60;
    //旋转角度
    ctx.rotate(theta);
    //开始绘制
    ctx.beginPath();
    //moveTo(x,y)：移动画笔到指定的坐标点(x,y)
    ctx.moveTo(-15, -4);
    //lineTo(x,y)：使用直线连接当前端点和指定的坐标点(x,y)
    ctx.lineTo(-15, 4);
    ctx.lineTo(clockRadius * 0.75, 1);
    ctx.lineTo(clockRadius * 0.75, -1);
    var canvasGradient = ctx.createLinearGradient(-15, -4, 0.6 * clockRadius, -15, 4, 0.6 * clockRadius);
    canvasGradient.addColorStop(0.5, 'black');
    canvasGradient.addColorStop(1, 'red');
    ctx.fillStyle = canvasGradient;
    ctx.fill();
    ctx.restore();
    //#endregion

    //#region 画秒针
    ctx.save();
    var theta = (seconds - 15) * 2 * Math.PI / 60;
    ctx.rotate(theta);
    ctx.beginPath();
    ctx.moveTo(-15, -3);
    ctx.lineTo(-15, 3);
    ctx.lineTo(clockRadius * 0.85, 1);
    ctx.lineTo(clockRadius * 0.85, -1);
    ctx.fillStyle = '#0f0';
    ctx.fill();
    ctx.restore();
    ctx.restore();
    //#endregion
}

// initialization
$(function () {
    canvas = document.getElementById('canvas');
    canvas.height = canvas.width;
    clockRadius = canvas.width / 2;

    ctx = canvas.getContext('2d');

    // var width = canvas.width;
    // var height = canvas.height;

    clockImage = new Image();
    clockImage.src = 'https://static.runoob.com/images/mix/125855_nnla_89964.png';

    setInterval(drawScene, 1000); // loop drawScene
});