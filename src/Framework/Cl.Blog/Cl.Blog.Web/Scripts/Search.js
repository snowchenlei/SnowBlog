//搜索
function Search(url) {
    var para = '{"pageIndex": 1';
    $("#search input").each(function () {
        if ($(this).val() != "") {
            para += ',"' + this.id.replace('Search', '') + '": "' + $(this).val() + '"';
        }
    });
    $("#search select").each(function () {
        para += ',"' + this.id.replace('Search', '') + '": "' + $(this).val() + '"';
    });
    var jPara = JSON.parse(para + '}');
    $.post(url, jPara, function (data) {
        Load(data, 1);
    });
}
//#region 时间段约束
$("#SearchStartTime").click(function () {
    $("#SearchStartTime").datetimepicker("setEndDate", $("#SearchEndTime").val())
});
$("#SearchEndTime").click(function () {
    $("#SearchEndTime").datetimepicker("setStartDate", $("#SearchStartTime").val())
});
//#endregion
//搜索重置
function Reset() {
    ClearData('search');
    $("#SearchStartTime").datetimepicker("setEndDate", '1900-01-01');
    $("#SearchEndTime").datetimepicker("setStartDate", '1900-01-01');
}

//添加
function Add_click() {
    ClearData("AddCategory");
}
//修改
function Edit_click(id) {
    ClearData("AddCategory");
    $.post("/Manager/Blogger/Detail", { "id": id }, function (data) {
        if (data.status == 1) {
            SetValue(data.result);
            $("#Id").val(id);
        } else {
            toastr.error(data.message);
        }
    });
}
//删除
function Delete_click(id) {
    $.post('/Manager/Blogger/Delete', { 'id': id }, function (data) {
        if (data.status == 1) {
            Load(data.result);
        } else {
            toastr.error(data.message);
        }
    });
}
//保存成功回调
function SaveSuccess(data) {
    if (data.status == 1) {
        $('#AddCategory').modal('hide');
        Load(data.result);
    } else {
        toastr.error(data.message);
    }
}