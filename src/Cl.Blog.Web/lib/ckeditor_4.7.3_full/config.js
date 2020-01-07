/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {
    config.language = 'zh-cn';
    config.removePlugins = 'elementspath';
    config.resize_enabled = false;
    config.htmlEncodeOutput = true;
    //config.baseHref = "CKEditor/";
    config.smiley_path = "CKEditor/plugins/smiley/images/";
    //config.toolbar = "Basic";
    config.font_names = "宋体;新宋体;仿宋体;黑体;楷体;隶书;幼圆;Arial;Comic Sans MS;Courier New;Fixedsys;Georgia;Tahoma;Times New Roman;Verdana";
    // config.uiColor = '#AADC6E';

    config.extraPlugins = 'clipboard,lineutils,widget,dialog,codesnippet';
    config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_P;

    //config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?Type=Images'; //上传图片时浏览服务文件夹
    //config.filebrowserFlashBrowseUrl = '/ckfinder/ckfinder.html?Type=Flash';  //上传Flash时浏览服务文件夹
    //config.filebrowserUploadUrl = '/api/Files/UploadFile'; //上传文件按钮(标签)
    config.filebrowserImageUploadUrl = '/api/Files/UploadImage'; //上传图片按钮(标签)
    //config.filebrowserImageUploadUrl = '/Manager/Blogger/UploadImage'; //上传图片按钮(标签)
    //config.filebrowserFlashUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash'; //上传Flash按钮(标签)

    //i.toolbar_Full=[['Source','-','Save','NewPage','Preview','-','Templates'],['Cut','Copy','Paste','PasteText','PasteFromWord','-','Print','SpellChecker','Scayt'],['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],['Form','Checkbox','Radio','TextField','Textarea','Select','Button','ImageButton','HiddenField'],'/',['Bold','Italic','Underline','Strike','-','Subscript','Superscript'],['NumberedList','BulletedList','-','Outdent','Indent','Blockquote','CreateDiv'],['JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],['BidiLtr','BidiRtl'],['Link','Unlink','Anchor'],['Image','Flash','Table','HorizontalRule','Smiley','SpecialChar','PageBreak','Iframe'],'/',['Styles','Format','Font','FontSize'],['TextColor','BGColor'],['Maximize','ShowBlocks','-','About']];
    config.toolbar_Basic = [
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord'],
        ['Link', 'Unlink', 'Anchor', 'Image', 'Flash', 'Table', 'CodeSnippet'],
        ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', 'RemoveFormat'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        '/',
        ['Font', 'FontSize', 'TextColor', 'BGColor'], ['Table', 'Maximize'],
        ['Source']
    ];

    config.toolbar_Full = [        
        ['Cut', 'Copy', 'Paste', 'Undo', 'Redo', 'Find', 'Replace', 'SelectAll', 'RemoveFormat'],
        ['Link', 'Unlink', 'Anchor', 'Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak', 'Iframe'],
        ['NumberedList', 'BulletedList', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv'],
        '/',
        ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Format', 'Font', 'FontSize', 'TextColor', 'BGColor', 'Maximize'], //, 'ShowBlocks'
        ['Source']
    ];
};
