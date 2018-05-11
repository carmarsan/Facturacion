$(document).ready(function () {


    var grid = $("#grid");
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';

    grid.jqGrid({
        //url: '@Url.Action("GetAll", "Ivas")',
        url: 'FormasPago/GetAll',
        datatype: 'json',
        mtype: 'GET',
        contentType: "application/json; charset-utf-8",
        colNames: ['Id', 'Descripción', 'Texto Largo', 'Vcto1', 'Vcto2', 'Vcto3', 'Vcto4', 'Vcto5', 'Vcto6','Mostrar', 'Fecha Alta'],
        colModel: [
            {
                name: 'FormaPagoId', key: true, hidden: false, index: 'FormaPagoId', editable: true, sortable: true, search: false, width: 46,
                editoptions: {
                    readonly: true,
                    dataInit: function (e) {
                        e.style.width = '60px';

                    }
                }
            },
            {
                name: 'Descripcion', key: false, index: 'Descripcion', editable: true, sortable: true, width: 300,
                editoptions: {
                    size: 50,
                    maxlength: 50,
                    dataInit: function (e) {
                        e.style.width = '300px';

                    }
                },
                editrules: {
                    required: true
                }
            },
            {
                name: 'TextoLargo', index: 'TextoLargo', editable: true, sortable: true, width: 300, resizable: true,
                editoptions: {
                    maxlength: 254,
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '500px';

                    }
                }
            },
            {
                name: 'Vcto1',  index: 'Vcto1',
                formatter: { 'currency': { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                editable: true, sortable: true, width: 80, resizable: true,
                editoptions: {
                    maxlength: 50,
                    number: true,
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '100px';

                    }
                }
            },
            {
                name: 'Vcto2', index: 'Vcto2',
                formatter: { 'currency': { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                editable: true, sortable: true, width: 80, resizable: true,
                editoptions: {
                    maxlength: 50,
                    number: true,
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '100px';

                    }
                }
            },
            {
                name: 'Vcto3', index: 'Vcto3',
                formatter: { 'currency': { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                editable: true, sortable: true, width: 80, resizable: true,
                editoptions: {
                    maxlength: 50,
                    number: true,
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '100px';

                    }
                }
            },
            {
                name: 'Vcto4', index: 'Vcto4',
                formatter: { 'currency': { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                editable: true, sortable: true, width: 80, resizable: true,
                editoptions: {
                    maxlength: 50,
                    number: true,
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '100px';

                    }
                }
            },
            {
                name: 'Vcto5', index: 'Vcto5',
                formatter: { 'currency': { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                editable: true, sortable: true, width: 80, resizable: true,
                editoptions: {
                    maxlength: 50,
                    number: true,
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '100px';

                    }
                }
            },
            {
                name: 'Vcto6', index: 'Vcto6',
                formatter: { 'currency': { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 } },
                editable: true, sortable: true, width: 80, resizable: true,
                editoptions: {
                    maxlength: 50,
                    number: true,
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '100px';

                    }
                }
            },
            {
                name: 'Mostrar', index: 'Mostrar', editable: true, align: 'center', search: false, edittype: 'checkbox', width: 69, formatter: 'checkbox',
                editoptions: {
                    value: "True:False",
                    dataInit: function (e) {
                        e.style.width = '13px';
                        e.style.textAlign = 'center';
                    }
                }
            },
            {   name: 'FechaAlta', key: false, index: 'FechaAlta', editable: false, hidden: true }],
        pager: jQuery('#pager'),
        rowNum: 20,
        rowList: [10, 20, 30, 40],
        height: 700,
        width: 720,
        viewrecords: true,
        caption: 'Listado de Formas de Pago',
        sortname: 'FormaPagoId',
        sortorder: "Asc",
        emptyrecords: 'No existen registros',
        ShrinkToFit: true,  // Para que ajuste las columnas automáticamente
        autowidth: true,
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
        }



    });


    grid.jqGrid('navGrid', '#pager', {
        edit: true,
        add: true,
        del: true,
        search: true,
        refresh: true,
        view: true,
        position: "left",
        cloneToTop: false
    }, Edit(), Add(), Delete());

    function Edit() {
        return {
            url: 'FormasPago/EditFormaPago',
            closeOnEscape: true,
            checkOnSubmit: true,
            closeAfterEdit: true,
            drag: true,
            modal: true,
            ShrinkToFit: true,
            savekey: [true, 13],
            width: 669,
            recreateForm: true,
            beforeShowForm: function (form) {
                centerDialog(grid);
            },
            afterComplete: function (response) {
                dialogo(response);
            }
        }
    }

    function Add() {
        return {
            zIndex: 100,
            url: "FormasPago/AddFormaPago",
            closeOnEscape: true,
            savekey: [true, 13],
            width: 669,
            modal: true,
            closeAfterAdd: true,
            reloadAfterSubmit: true,
            drag: true,
            beforeShowForm: function (form) {
                $('#tr_FormaPagoId', form).hide();
                $('#Mostrar', form).prop("checked", true);

                centerDialog(grid);
            },
            afterComplete: function (response) {
                dialogo(response);
            }
        }
    }

    function Delete() {
        return {
            zIndex: 100,
            url: "FormasPago/DeleteFormaPago",
            closeOnEscape: true,
            closeAfterDelete: true,
            modal: true,
            recreateForm: true,
            left: 400,
            top: 400,
            drag: true,
            savekey: [true, 13],
            width: 416,
            msg: "Está seguro de querer borrar este registroooo?",
            afterComplete: function (response) {
                dialogo(response);
            },
            beforeShowForm: function () {
                centerDialog(grid);
            }

        }
    }

});


