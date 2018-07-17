//分页配置初始化
function InitPageOption(pageIndex, pageCount) {
    //$("#totalCount").text(data.totalCount);
    //$("#pageIndex").text(data.pageIndex);
    //$("#pageCount").text(data.pageCount);
    var options = {
        bootstrapMajorVersion: 2, //版本
        currentPage: pageIndex, //当前页数
        totalPages: pageCount, //总页数
        numberOfPages: 5,
        alignment: 'center',
        itemTexts: function (type, page, current) {
            switch (type) {
                case "first":
                    return "首页";
                case "prev":
                    return "上一页";
                case "next":
                    return "下一页";
                case "last":
                    return "末页";
                case "page":
                    return page;
            }
        },//点击事件，用于通过Ajax来刷新整个list列表
        onPageClicked: function (event, originalEvent, type, page) {
            pageClicked(page);
            //$.post("/Blogger/List", { "pageIndex": page }, function (data) {
            //    $("#page").val(page);
            //    ReplaceData(data.list);
            //    ReplaceParent(data.parentData, "PId");
            //});
        }
    };
    $('#pageDetail').bootstrapPaginator(options);
}
//下拉选绑定
function ReplaceParent(data, selectId) {
    
    var htmlStr = '<option value="-1">请选择</option>';
    for (var i = 0, max = data.length; i < max; i++) {
        htmlStr += '<option value=' + data[i].Val + '>' + data[i].Name + '</option>';
    }
    var selectIds = selectId.split(',');
    for (var i = 0, max = selectIds.length; i < max; i++) {
        $('#' + selectIds[i] + ' option').remove();
        $('#' + selectIds[i]).append(htmlStr);
    }
    //刷新控件，否则无法显示
    $('.selectpicker').selectpicker('refresh');
}
//数据初始化
function ClearData(id) {
    $('#' + id + ' input').val('');
    $('#' + id + ' select').selectpicker('val', -1);
    $('#' + id + ' input[type="checkbox"]').prop("checked", "checked");
    $('#' + id + ' input[type="checkbox"]').val("true");
    $('#' + id + ' input[type="hidden"]').val('');
    $('#' + id + ' textarea').val('');
}
//多选框选中改变
function checkbox_change(e) {
    if ($(e).is(':checked')) {
        $(e).prop("checked", true);
        $(e).val("true");
    } else {
        $(e).prop("checked", false);
        $(e).val("false");
    }
}
//文本框是否为空
function IsNullOrEmpty(id) {
    if ($("#" + id).val() == "") {
        return false;
    } else {
        return true;
    }
}

//#region值获取
function TxtVal(id) {
    return $("#" + id).val();
}
function CheckboxVal(id) {
    return $("#" + id).val();
}
function SelectVal(id) {
    return $("#" + id).val();
}
//#endregion
//多选框选中
function SetCheckbox(id, val) {
    if (val == true) {
        $('#' + id).prop('checked', true);
        $('#' + id).val(true);
    } else {
        $('#' + id).prop('checked', false);
        $('#' + id).val(false);
    }
}