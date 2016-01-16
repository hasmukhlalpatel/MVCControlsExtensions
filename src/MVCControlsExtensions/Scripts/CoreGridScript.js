

function AddNewRow(element) {
    var table = findParentTag(element, "TABLE");
    var tbody = table.tBodies[0];
    var template = tbody.getAttribute('data-template');
    var newRowId = table.rows.length > 1 ? Number(table.rows[table.rows.length - 1].id) + 1 : 1001;

    template = template.replaceAll('{0000}', newRowId).replaceAll('_0000_', newRowId);
    var row = table.insertRow(table.rows.length);
    row.outerHTML = template;
}

function DeleteRow(element) {
    var table = findParentTag(element, "TABLE");
    var tr = findParentTag(element, "TR");

    for (var i = 0; i < table.rows.length; i++) {
        if (table.rows[i] == tr) {
            table.deleteRow(i);
            break;
        }
    }
}

function findParentTag(element, tagName) {
    if (element.parentNode == null)
        return null;

    if (element.parentNode.tagName == tagName) {
        return element.parentNode;
    } else {
        return findParentTag(element.parentNode, tagName);
    }
}