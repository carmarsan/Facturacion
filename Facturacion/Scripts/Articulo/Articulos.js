var grid = $("#grid");

$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';

$(document).ready(function () {

    grid.jqGrid({
        //url: '@Url.Action("GetAll", "Ivas")',
        url: 'Articulos/GetAll',
        datatype: 'json',
        mtype: 'GET',
        contentType: "application/json; charset-utf-8",
        colNames: colnames,
        colModel: colmodel,
        pager: jQuery('#pager'),
        // Estas dos opciones son para que salga el listado con todos los registros y cada vez que se haga scroll pida los datos al servidor
        //page: 1,
        //scroll: 1,
        autowidth: true,
        rowNum: 20,
        //loadui: "Bootstrap",
        styleUI: 'Bootstrap',
        rowList: [10, 20, 30, 40],
        height: 700,
        width: 720,
        viewrecords: true,
        caption: 'Listado de Artículos',
        sortname: 'ArticuloId',
        sortorder: "Asc",
        emptyrecords: 'No existen registros',
        ShrinkToFit: true,  // Para que ajuste las columnas automáticamente
        autowidth: true,
        viewrecords: true,
        loadonce: false,    // Sólo carga una vez todos los datos.
        multiselect: false,
        loadError: function (xhr, status, err) {
            //jQuery("#rsperror").show().html("Type: " + status + "; Response: " + xhr.status + " " + xhr.statusText);
            alert(xhr);
            //		try {
            //			jQuery.jgrid.info_dialog(jQuery.jgrid.errors.errcap, '' + xhr.responseText +'', jQuery.jgrid.edit.bClose,{buttonalign:'right'});
            //		} catch(e) {
            //			alert(xhr.responseText);
            //		}
        },
        loadComplete: function (data) {
            //alert(data);
        },
        rowattr: function () { // Si es un Abono le añadimos la clase que da color al fondo de la fila
            return { 'class': 'hasSubmenu' }
    }

    });

    grid.jqGrid('navGrid', '#pager', {
        position: "left",
        cloneToTop: false
    }, Edit, Add, Delete, Buscar);

    

});

/*
 * Opciones de los botons del pager
 */
var Edit = {
        // edit options
        url: 'Articulos/EditArticulo',
        closeOnEscape: true,
        checkOnSubmit: true,
        closeAfterEdit: true,
        modal: true,
        ShrinkToFit: true,
        savekey: [true, 13],
        width: 400,
        recreateForm: true,
        beforeShowForm: function (form) {
            setEditDialogCapture("Modificar Artículo", "Edit", grid, -1);
            centerDialog(grid);
        },
        afterComplete: function (response) {
            dialogo(response);
        },
        afterclickPgButtons: function () {
            setEditDialogCapture("Modificar Artículo", "Edit", grid, -1);
        },
},
Add = {
        // add options
        zIndex: 100,
        url: "Articulos/AddArticulo",
        closeOnEscape: true,
        savekey: [true, 13],
        width: 400,
        modal: true,
        closeAfterAdd: true,
        reloadAfterSubmit: true,
        drag: true,
        beforeShowForm: function (form) {
            $('#tr_ArticuloId', form).hide();
            $('#Ver', form).prop("checked", true);

            centerDialog(grid);
        },
        afterComplete: function (response) {
            dialogo(response);
        }
},
Delete = {
        zIndex: 100,
        url: "Articulos/DeleteArticulo",
        closeOnEscape: true,
        closeAfterDelete: true,
        modal: true,
        recreateForm: true,
        left: 400,
        top: 400,
        savekey: [true, 13],
        width: 416,
        msg: "Está seguro de querer borrar este registroooo?",
        afterComplete: function (response) {
            dialogo(response);
        },
        beforeShowForm: function (form) {
            centerDialog(grid);
        }

},
Buscar = {
    showQuery: false,
    closeOnEscape: false,
    recreateForm: true,
    searchOnEnter: true,
    defaultSearch: "cn"
}

var colnames = ['Id', 'Descripción', 'Precio', 'Precio Compra', 'Precio Venta', 'IVA', 'Cuenta Contable', 'Mostrar', 'Fecha Alta'],
    colmodel = [
            {
                name: 'ArticuloId', key: true, hidden: false, index: 'ArticuloId', editable: true, sortable: true, search: false, width: 46, editoptions: { readonly: true, }
            },
            {
                name: 'Descripcion', key: false, index: 'Descripcion', editable: true, sortable: true, width: 450,
                editoptions: {
                    size: 15,
                    maxlength: 150,
                    dataInit: function (e) {
                        e.style.width = '200px';

                    }
                },
                editrules: {
                    required: true
                }
            },
            {
                name: 'Precio', index: 'Precio',
                formatter: { 'currency': { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                editable: true, sortable: true, width: 110, resizable: true,
                editoptions: {
                    //size: 50,
                    maxlength: 50,
                    number: true,
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '100px';

                    }
                },
                editrules: {
                    required: true
                }
            },
            {
                name: 'PrecioCompra',     //key: false,
                index: 'PrecioCompra',
                formatter: { 'number': { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                editable: true, sortable: true, width: 110, resizable: true,
                editoptions: {
                    maxlength: 50,
                    number: true,
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '100px';

                    }
                },
                editrules: {
                    required: true
                }
            },
            {
                name: 'PrecioVenta', index: 'PrecioVenta',
                formatter: { 'number': { decimalSeparator: ".", thousandsSeparator: ".", decimalPlaces: 2 } },
                editable: true, sortable: true, width: 110, resizable: true,
                editoptions: {
                    maxlength: 50,
                    number: true,
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '100px';

                    }
                },
                editrules: {
                    required: true
                }
            },
            {
                name: 'IvaId', index: 'IvaId', sortable: true, edittype: "select", editable: true, width: 90,
                editoptions: {
                    value: GetIvas,
                    dataInit: function (e) {
                        e.style.width = '100px';
                        e.style.height = '23.59px';
                    },
                },
                editrules: {
                    required: true
                }
            },
            {
                name: 'CodigoContable', key: false, index: 'CodigoContable', editable: true, width: 140,
                editoptions: {
                    //size: 50,
                    maxlength: 25,
                    dataInit: function (e) {
                        e.style.width = '180px';

                    }
                }
            },
            {
                name: 'Ver', index: 'Ver', editable: true, align: 'center', search: false, edittype: 'checkbox', width: 69, formatter: 'checkbox',
                editoptions: {
                    value: "True:False",
                    dataInit: function (e) {
                        e.style.width = '13px';
                        e.style.textAlign = 'center';
                    }
                }
            },

            { key: false, name: 'FechaAlta', index: 'FechaAlta', editable: false, hidden: true }];


var GetIvas = function () {
    var selectIvas;
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: "",
        async: false,
        url: "/Ivas/GetSelectIvas",
        dataType: "json",
        success: function (response) {
            selectIvas = response;

            var data = typeof response === "string" ? JSON.stringify(response) : response;
            var s = "";

            $.each(data, function (i, item) {

                s += item.Id + ":" + item.Texto + ";"; // '<option value="' + item.Id + '">' + item.Texto + '</option>';
            })
            selectIvas = s.substring(0, s.length - 1);
        },

        error: function (XMLHttpRequest, textStatus, errorThrown) {
            debugger;
        }

    });
    return selectIvas;
    //return JSON.stringify(selectIvas);

}

/*
 *  SubMenu en el grid
 */
var submenu = new BootstrapMenu(".hasSubmenu",
{
    fetchElementData: function ($rowElem) {
        return $rowElem[0];
    },
    actionsGroups: [
        ['Nueva', 'Modificar'], ['Borrar'], ['Imprimir']
    ],
    actions: {
        Nueva: {
            name: 'Nuevo Artículo',
            iconClass: 'glyphicon glyphicon-plus',
            onClick: function () {
                grid.editGridRow("new", Add);
            }
        },
        Modificar: {
            name: 'Modificar Artículo',
            iconClass: 'glyphicon glyphicon-edit',
            onClick: function (row) {
                var rowKey = grid.getGridParam("selrow");
                grid.editGridRow(rowKey, Edit);
            }
        },
        Borrar: {
            name: 'Borrar Artículo',
            iconClass: 'glyphicon glyphicon-trash',
            onClick: function () {
                var rowKey = grid.getGridParam("selrow");
                grid.delGridRow(rowKey, Delete);
            }
        },
        Imprimir: {
            name: 'Imprimir',
            iconClass: 'glyphicon glyphicon-print',
            onClick: function () {

            }
        },
    }
});
