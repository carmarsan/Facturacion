var setEditDialogCapture = function (texto, funcion, grid, index) {
    var title = $(".ui-jqdialog-title"); //$("#edithd" + grid[0].id + " .ui-jqdialog-title");

    if (funcion == "Edit" || funcion == "Del") {
        if (index != -1) {
            var sel_id = grid.jqGrid('getGridParam', 'selrow');
            //var selRowData = grid.jqGrid('getRowData', sel_id);
            var selRowData = grid.jqGrid('getCell', sel_id, index);

            //var nombre = selRowData.NombreCliente;

            //title.text(texto + " '" + selRowData.NombreCliente + "'");
            title.text(texto + " '" + selRowData + "'");
        }
        else
            title.text(texto);
    }
    else
        title.text(texto);
};

var dialogo = function (response) {
    if (response.responseText) {
        var respuesta = response.responseJSON;

        jQuery.jgrid.info_dialog("Info", "<div class=\"ui-state-highlight\" style=\"padding:5px;\">" + respuesta.Msg + "</div>",
												jQuery.jgrid.edit.bClose, { buttonalign: "right" });
        jQuery("#info_dialog").delay(3000).fadeOut();
    }

};

var centerDialog = function (grid)
{
    var dlgDiv = $("#editmod" + grid[0].id);
    if (dlgDiv.length == 0)
        dlgDiv = $("#delmod" + grid[0].id);
    var parentDiv = $("body");
    var dlgWidth = dlgDiv.width();
    var dlgHeight = dlgDiv.height();
    var parentWidth = parentDiv.width();
    var parentHeight = parentDiv.height();

    var top = Math.round((parentHeight - dlgHeight) / 2) + "px";
    var left = Math.round((parentWidth - dlgWidth) / 2) + "px";

    dlgDiv[0].style.top = Math.round((parentHeight - dlgHeight) / 2) + "px";
    dlgDiv[0].style.left = Math.round((parentWidth - dlgWidth) / 2) + "px";

    //return [top, left];
}

var formatStrintToNumber = function(amount, decimals, sign)
{
    amount += ''; // por si pasan un numero en vez de un string
    amount = parseFloat(amount.replace(/[^0-9\.]/g, '')); // elimino cualquier cosa que no sea numero o punto

    decimals = decimals || 0; // por si la variable no fue fue pasada

    // si no es un numero o es igual a cero retorno el mismo cero
    if (isNaN(amount) || amount === 0)
        return parseFloat(0).toFixed(decimals);

    // si es mayor o menor que cero retorno el valor formateado como numero
    amount = '' + amount.toFixed(decimals);

    var amount_parts = amount.split('.'),
        regexp = /(\d+)(\d{3})/;

    while (regexp.test(amount_parts[0]))
        amount_parts[0] = amount_parts[0].replace(regexp, '$1' + '.' + '$2');

    if (sign == "")
        return amount_parts.join(',');
    else
        return amount_parts.join(',') + " " + sign;
}
