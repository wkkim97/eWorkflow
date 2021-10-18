function getDataItemKeyValue(radGrid, item, name) {
    return radGrid.get_masterTableView().getCellByColumnUniqueName(item, name).innerHTML;
}


function fn_MailBodyShowDocument(url) {
    var nWidth = 935;
    var nHeight = 680;
    var left = (screen.width / 2) - (nWidth / 2);
    var top = (screen.height / 2) - (nHeight / 2) - 10;
    window.open(url, "", "width=" + nWidth + "px, height=" + nHeight + "px, top=" + top + "px, left=" + left + "px,location=no,titlebar=no,status=no,scrollbars=yes,menubar=no,toolbar=no,directories=no,resizable=no,copyhistory=no");
}