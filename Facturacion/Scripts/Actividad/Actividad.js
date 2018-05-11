$(function () {

    var grid = $("#grid");
    $.jgrid.defaults.responsive = true;
    $.jgrid.defaults.styleUI = 'Bootstrap';

    grid.jqGrid({
        url: "/Actividades/GetAll",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['ActividadID', 'Nombre', 'Abreviatura', 'Mostrar', 'Fecha Alta'],
        colModel: [
            { key: true, hidden: true, name: 'ActividadID', index: 'ActividadID', editable: true },
            { key: false, name: 'Nombre', index: 'Nombre', editable: true, sortable: true, firstsortorder: 'asc' },
            {
                key: false, name: 'Abreviatura', index: 'Abreviatura', editable: true, width: 20,
                editoptions: {
                    maxlength: 3,
                }
            },
            {
                key: false, name: 'Mostrar', index: 'Mostrar', editable: true, align: 'center', width: 20, edittype: 'checkbox',
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
                name: 'FechaAlta', editable: false, hidden: true
            }],
        pager: jQuery('#pager'),
        rowNum: 10,
        rowList: [10, 20, 30, 40],
        height: '100%',
        viewrecords: true,
        caption: 'Actividades',
        sortorder: "Asc",
        sortname: 'ActividadID',
        styleUI: 'Bootstrap',
        emptyrecords: 'No existen registros',
        width: 900,
        viewrecords: true,
        autowidth: true,
        multiselect: false
    }).navGrid('#pager', { edit: true, add: true, del: true, search: false, refresh: true },
        {
            // edit options
            zIndex: 100,
            url: '/Actividades/EditActividad',
            savekey: [true, 13],
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                dialogo(response);
            },
            beforeShowForm: function () {
                setEditDialogCapture("Edición del cliente", "Edit", grid);
                centerDialog(grid);
            },
            afterclickPgButtons: function () {
                setEditDialogCapture("Edición del cliente", "Edit", grid);
            },
        },
        {
            // add options
            zIndex: 100,
            url: "/Actividades/AddActividad",
            savekey: [true, 13],
            closeOnEscape: true,
            closeAfterAdd: true,
            afterComplete: function (response) {
                dialogo(response);
            },
            beforeShowForm: function (form) {
                $('#Mostrar', form).prop("checked", true);
                setEditDialogCapture("Nueva actividad", "Add", grid);
                centerDialog(grid);

            },

        },
        {
            // delete options
            zIndex: 100,
            url: "/Actividades/DeleteActividad",
            savekey: [true, 13],
            closeOnEscape: true,
            closeAfterDelete: true,
            jqModal: true,
            modal: true,
            recreateForm: true,
            left: 400,
            top: 400,
            width: 416,
            //height: 250,
            msg: "Está seguro de querer borrar este registro?",
            afterComplete: function (response) {
                dialogo(response);
            },
            beforeShowForm: function (form) {
                centerDialog(grid);
            }
        });
});