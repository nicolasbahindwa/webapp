function confirmDelete(uniqueid, isDeleteClicked)
{
    console.log('clicked');
    var deleteSpan = 'deleteSpan_'+ uniqueid;
    var confirmDeleteSpan = 'confirmDeleteSpan_'+ uniqueid;

    if (isDeleteClicked) {
        console.log(isDeleteClicked);
        $('#'+ deleteSpan).hide();
        $('#'+ confirmDeleteSpan).show();
    }
    else {
        $('#'+ deleteSpan).show();
        $('#'+ confirmDeleteSpan).hide();
    }

}