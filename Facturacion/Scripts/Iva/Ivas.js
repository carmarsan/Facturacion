$(document).ready(function () {

    var grid = $("#grid");
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';

    $("#grid").jqGrid({
            url: 'Ivas/GetAll',
            datatype: 'json',
            mtype: 'GET',
            contentType: "application/json; charset-utf-8",
            colNames: ['Id', 'Texto', 'Porcentaje', 'Cuenta Contable', 'Defecto', 'Mostrar','Fecha Alta'],
            colModel: [
                {
                    name: 'IvaId',
                    key: true,
                    hidden: false,
                    index: 'IvaId',
                    editable: true,
                    sortable: true,
                    search: false,
                    width: 46,
                    editoptions: { readonly: true }
                },
                {
                    name: 'Texto',
                    key: false,
                    index: 'Texto',
                    editable: true,
                    sortable: true,
                    width: 76,
                    editoptions: {
                        size: 15,
                        maxlength: 8,
                        dataInit: function (e) {
                            e.style.width = '120px';

                        }
                    },
                    editrules: {
                        required: true
                    }
                },
                {
                    name: 'Porcentaje',
                    index: 'Porcentaje',
                    formatter: { 'currency': { decimalSeparator: ".", thousandsSeparator: ".", decimalPlaces: 2, suffix: " %" } },
                    editable: true,
                    sortable: true,
                    width: 150,
                    resizable: true,
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
                    label: 'CuentaContable',
                    name: 'CuentaContable',
                    key: false,
                    index: 'CuentaContable',
                    editable: true,
                    width: 179,
                    editoptions: {
                        //size: 50,
                        maxlength: 200,
                        dataInit: function (e) {
                            e.style.width = '180px';

                        }
                    }
                },
                {
                    name: 'Defecto',
                    key: false,
                    index: 'Defecto',
                    editable: true,
                    align: 'center',
                    search: false,
                    //width: 13,
                    edittype: 'checkbox',
                    width: 69,
                    editoptions: {
                        value: "True:False",
                        dataInit: function (e) {
                            e.style.width = '13px';
                            e.style.textAlign = 'center';
                        }
                    },
                    formatter: 'checkbox'
                },
                {
                    name: 'Mostrar',
                    key: false,
                    index: 'Mostrar',
                    editable: true,
                    align: 'center',
                    search: false,
                    //width: 13,
                    edittype: 'checkbox',
                    width: 69,
                    editoptions: {
                        value: "True:False",
                        dataInit: function (e) {
                            e.style.width = '13px';
                            e.style.textAlign = 'center';
                        }
                    },
                    formatter: 'checkbox'
                },
                { key: false, name: 'FechaAlta', index: 'FechaAlta', editable: false, hidden: true }],
            pager: jQuery('#pager'),
            autowidth: true,
            rowNum: 20,
            styleUI: 'Bootstrap',
            rowList: [10, 20, 30, 40],
            height: 600,
            width: 220,
            viewrecords: true,
            caption: 'Listado de Iva',
            sortname: 'IvaId',
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
            }


        });

        $("#grid").jqGrid('navGrid', '#pager', {}, Edit(), Add(), Delete());

    function Edit() {
        return {
            // edit options
            zIndex: 100,
            url: 'Ivas/EditIva',
            closeOnEscape: true,
            checkOnSubmit: true,
            closeAfterEdit: true,
            modal: true,
            ShrinkToFit: true,
            savekey: [true, 13],
            width: 400,
            recreateForm: true,
            // Antes de mostrar el diálogo, miro si es un registro que tiene el campo Defecto a True y hago que no se pueda modificar
            beforeShowForm: function (form) {
                var selRowId = $("#grid").jqGrid('getGridParam', 'selrow'),
                    IsDefecto = $("#grid").jqGrid('getCell', selRowId, 'Defecto');
                $('#Defecto', form).prop("disabled", (IsDefecto == "True") ? true : false);
                setEditDialogCapture("Edición del cliente", "Edit", grid);
                centerDialog(grid);
            },
            // Cuando damos a los botones del formulario (prev, next).  En row, viene el número de fila pero al que se va, sino en el que está antes de saltar
            onclickPgButtons: function(button, form, row){
                var  IsDefecto = grid.jqGrid('getCell', button == "next" ? ++row : --row, 'Defecto');
                $('#Defecto', form).prop("disabled", (IsDefecto == "True") ? true : false);
            },
            afterComplete: function (response) {
                dialogo(response);
            }
        }
    }

    function Add() {
        return {
            // add options
            zIndex: 100,
            //url: '@Url.Action("AddBancos", "Bancos")',
            url: "Ivas/AddIva",
            //url: "api/Bancos/",
            closeOnEscape: true,
            savekey: [true, 13],
            width: 400,
            modal: true,
            closeAfterAdd: true,
            reloadAfterSubmit: true,
            drag: true,
            beforeShowForm: function (form) {
                $('#tr_IvaId', form).hide();
                $('#Mostrar', form).prop("checked", true);
                setEditDialogCapture("Nueva actividad", "Add", grid);
                centerDialog(grid);
            },
            afterComplete: function (response) {
                dialogo(response);
            }
        }
    }

    function Delete() {
        return {
            // delete options
            zIndex: 100,
            url: "Ivas/DeleteIva",
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

        }
    }

        
});

