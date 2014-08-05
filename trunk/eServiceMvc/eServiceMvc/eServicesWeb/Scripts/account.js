$("#editName").click(function() {
    hideAllEditors();
    $('#displayName').hide();
    $('#editorName').show();
    $('#editorName .editLink a').show();
    $('#FirstName').select().focus();
});
$("#cancelName").click(function () {
    $('#displayName').show();
    $('#editorName').hide();
    showAllEditors();
});

$("#editBirthday").click(function () {
    hideAllEditors();
    $('#displayBirthday').hide();
    $('#editorBirthday').show();
    $('#editorBirthday .editLink a').show();
});
$("#cancelBirthday").click(function() {
    $('#displayBirthday').show();
    $('#editorBirthday').hide();
    showAllEditors();
});

$("#editEmail").click(function () {
    hideAllEditors();
    $('#displayEmail').hide();
    $('#editorEmail').show();
    $('#editorEmail .editLink a').show();
    $('#Email').select().focus();
});
$("#cancelEmail").click(function () {
    $('#displayEmail').show();
    $('#editorEmail').hide();
    showAllEditors();
});

$("#editMobile").click(function () {
    hideAllEditors();
    $('#displayMobile').hide();
    $('#editorMobile').show();
    $('#editorMobile .editLink a').show();
    $('#Mobile').select().focus();
});
$("#cancelMobile").click(function () {
    $('#displayMobile').show();
    $('#editorMobile').hide();
    showAllEditors();
});

$(".saveForm").click(function () {
    $('#userProfileForm').submit();
});

function hideAllEditors() {
    $(".editLink a").hide();
}
function showAllEditors() {
    $(".editLink a").show();
}