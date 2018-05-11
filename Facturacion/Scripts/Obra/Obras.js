var grid = $("#grid");


$(document).ready(function () {


	// Parámetros globales del grid
	$.jgrid.defaults.responsive = true;
	$.jgrid.defaults.styleUI = 'Bootstrap';

	grid.jqGrid({
		url: 'Obras/GetAll',
		datatype: 'json',
		mtype: 'GET',
		gridview: true,
		contentType: "application/json; charset-utf-8",
		colNames: colnames,
		colModel: colmodel,
		pager: jQuery('#pager'),
		autowidth: true,
		rowNum: 20,
		rowList: [10, 20, 30, 40],
		height: 600,
		viewrecords: true,
		caption: 'Listado de Obras',
		sortname: 'ClienteId',
		sortorder: "Asc",
		emptyrecords: 'No existen registros',
		ShrinkToFit: true,  // Para que ajuste las columnas automáticamente
		headertitles: true,
		sortIconsBeforeText: true,
		iconSet: "fontAwesome",
		loadonce: false,    // Sólo carga una vez todos los datos.
		multiselect: false,
		loadError: function (xhr, status, err) {
			dialogo(xhr);
		},
		loadComplete: function (data) {
		    dialogo("Esto es toooodooooo");
		},
		rowattr: function (rd) { // Si es un Abono le añadimos la clase que da color al fondo de la fila
		    if (rd.Acabada === 'True')
		        return { 'class': 'isObraCerrada hasSubmenu' };
		    else
		        return { 'class': 'hasSubmenu' }
		},
		onSelectRow: function (rowid, stat, e) {
		    var obra = $(this).jqGrid('getRowData', rowid);
		    $("#btnCerrar").prop("disabled", obra.Acabada == 'True');
		}
	});

	grid.jqGrid('navGrid', '#pager', { /*search: true,  add: true, edit: true, del: true*/
	}, EditGrid, AddGridRow, DeleteRow, Buscar);

	grid.jqGrid('bindKeys');
	grid.jqGrid('setLabel', 'Mostrar', '', { 'text-align': 'center' });

});

var AddGridRow = {
    zIndex: 100,
    width: 1236,
    url: "Obras/AddObra",
    closeOnEscape: true,
    savekey: [true, 13],
    modal: true,
    closeAfterAdd: true,
    reloadAfterSubmit: true,
    editCaption: "Nuevo Obra",
    drag: true,
    beforeShowForm: function (form) {
        $('#Mostrar', form).prop("checked", true);
        setEditDialogCapture("Nuevo Obra", "Add", grid, -1);

        centerDialog(grid);
    },
    afterComplete: function (response) {
        dialogo(response);
    }
},
EditGrid = {
    url: 'Obras/EditObra',
    closeOnEscape: true,
    checkOnSubmit: true,
    closeAfterEdit: true,
    drag: true,
    modal: true,
    caption: "Edición",
    ShrinkToFit: true,
    savekey: [true, 13],
    width: 1236,
    recreateForm: true,
    //afterShowForm: RellenaPoblacion,
    beforeShowForm: function () {
        setEditDialogCapture("Edición de la Obra", "Edit", grid, -1);
        centerDialog(grid);
    },
    afterclickPgButtons: function () {
        setEditDialogCapture("Edición de la Obra", "Edit", grid, -1);
    },
    afterComplete: function (response) {
        dialogo(response);
    }
},
Buscar = {
    //multipleSearch: false,
    //multipleGroup: false,
    showQuery: false,
    closeOnEscape: false,
    //recreateForm: true,
    searchOnEnter: true,
    defaultSearch: "cn"
},
DeleteRow = {
    zIndex: 100,
    url: "Obras/DeleteObra",
    closeOnEscape: true,
    closeAfterDelete: true,
    modal: true,
    recreateForm: true,
    left: 400,
    top: 400,
    drag: true,
    savekey: [true, 13],
    width: 416,
    msg: "Está seguro de querer borrar este registro?",
    beforeShowForm: function (form) {
        setEditDialogCapture("Borrar Obra", "Del", grid, 1);
        centerDialog(grid);
    },
    afterComplete: function (response) {
        dialogo(response);
    }
};

var colnames = ['Id', 'Nombre', 'Nombre Ampliado', 'Dirección', 'Localidad', 'CP', 'Provincia', 'Teléfono', 'Móvil', 'Fax', 'Técnico', 'Observaciones', 'Mostrar', 'Certi. Ant', 'Contar Abonos', 'Acabada', 'A Cód.Obra', 'Actividad', 'SubActividad', 'Delegación', 'Cliente', 'Fecha Alta'],
    colmodel = [
			{
			    name: 'ObraId', key: true, index: 'ObraId', editable: true, sortable: true, search: false, width: 90,
			    formoptions: { rowpos: 1, colpos: 1 },
			    sorttype: 'number',
			    editoptions: {
			        readonly: true,
			        dataInit: function (e) {
			            e.style.width = '90px';

			        }
			    }
			},
			{
			    name: 'Nombre', index: 'Nombre', editable: true, sortable: true, width: 650, resizable: true,
			    formoptions: { rowpos: 2, colpos: 1 },
			    sorttype: 'text',
			    searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
			    formoptions: { elmuffix: ' *' },
			    editoptions: {
			        maxlength: 100,
			        //edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '550px';

			        }
			    }
			},
			{
			    name: 'NombreAmpliado', index: 'NombreAmpliado', editable: true, sortable: true, width: 1050, resizable: true,
			    formoptions: { rowpos: 3, colpos: 1 },
			    searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
			    editoptions: {
			        maxlength: 100,
			        //edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '550px';

			        }
			    }
			},
			{
			    name: 'Direccion', index: 'Direccion', editable: true, sortable: true, width: 450, resizable: true,
			    formoptions: { elmuffix: ' *', rowpos: 4, colpos: 1 },
			    searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
			    editoptions: {
			        maxlength: 100,
			        //edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '400px';

			        }
			    }
			},
            {
                name: 'Localidad', index: 'Localidad', editable: true, sortable: true, width: 450, resizable: true,
                formoptions: { rowpos: 4, colpos: 2 },
                searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
                editoptions: {
                    maxlength: 100,
                    //edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '350px';

                    }
                }
            },
			{
			    name: 'CP', index: 'CP', editable: true, sortable: true, width: 110, resizable: true,
			    formoptions: { rowpos: 5, colpos: 1 },
			    searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
			    editoptions: {
			        maxlength: 100,
			        dataInit: function (e) {
			            e.style.width = '90px';

			        }
			    }
			},
            {
                name: 'Provincia', index: 'Provincia', editable: true, sortable: true, width: 210, resizable: true,
                formoptions: { rowpos: 5, colpos: 2 },
                searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
                editoptions: {
                    maxlength: 100,
                    //edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '200px';

                    }
                }
            },
			{
			    name: 'Telefono', index: 'Telefono', editable: true, sortable: true, width: 210, resizable: true,
			    formoptions: { rowpos: 6, colpos: 1 },
			    editoptions: {
			        maxlength: 100,
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '120px';

			        }
			    }
			},
			{
			    name: 'Movil', index: 'Movil', editable: true, sortable: true, width: 210, resizable: true,
			    formoptions: { rowpos: 6, colpos: 2 },
			    editoptions: {
			        maxlength: 100,
			        //edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '120px';

			        }
			    }
			},
			{
			    name: 'Fax', index: 'Fax', editable: true, sortable: true, width: 210, resizable: true, hidden: true,
			    formoptions: { rowpos: 7, colpos: 1 },
			    editoptions: {
			        maxlength: 100,
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '120px';

			        }
			    },
			    editrules: { edithidden: true }
			},
			{
			    name: 'Tecnico', index: 'Tecnico', editable: true, sortable: true, width: 280, resizable: true, hidden: true,
			    formoptions: { rowpos: 7, colpos: 2 },
			    editoptions: {
			        maxlength: 100,
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '300px';

			        }
			    },
			    editrules: {
			        edithidden: true,
			    }
			},
			{
			    name: 'Observaciones', index: 'Observaciones', edittype: 'textaera', editable: true, sortable: true, width: 250, resizable: true, hidden: true,
			    formoptions: { rowpos: 8, colpos: 1 },
			    editoptions: {
			        rows: "5", cols: "50",
			        maxlength: 100,
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '450px';

			        }
			    },
			    editrules: {
			        edithidden: true,
			    }
			},
            {
                name: 'Mostrar', index: 'Mostrar', editable: true, align: 'center', search: false, edittype: 'checkbox', width: 160, formatter: 'checkbox',
                formoptions: { rowpos: 8, colpos: 2 },
                editoptions: {
                    value: "True:False",
                    dataInit: function (e) {
                        e.style.width = '13px';
                        e.style.textAlign = 'center';
                    }
                }
            },
            {
                name: 'CertificacionesAnteriores', index: 'CertificacionesAnteriores', editable: true, align: 'center', search: false, edittype: 'checkbox', width: 160, formatter: 'checkbox', hidden: true,
                formoptions: { rowpos: 9, colpos: 1 },
                editoptions: {
                    value: "True:False",
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '13px';
                        e.style.textAlign = 'center';
                    }
                },
                editrules: { edithidden: true }
            },
            {
                name: 'ContarAbonos', index: 'ContarAbonos', editable: true, align: 'center', search: false, edittype: 'checkbox', width: 160, formatter: 'checkbox', hidden: true,
                formoptions: { rowpos: 9, colpos: 2 },
                editoptions: {
                    value: "True:False",
                    edithidden: true,
                    dataInit: function (e) {
                        e.style.width = '13px';
                        e.style.textAlign = 'center';
                    }
                },
                editrules: { edithidden: true }
            },
            {
                name: 'Acabada', index: 'Acabada', editable: true, align: 'center', search: false, edittype: 'checkbox', width: 160, formatter: 'checkbox', hidden: true,
                formoptions: { rowpos: 10, colpos: 1 },
                editoptions: {
                    edithidden: true,
                    value: "True:False",
                    dataInit: function (e) {
                        e.style.width = '13px';
                        e.style.textAlign = 'center';
                    }
                }
            },
            {
                name: 'ACodigoObra', index: 'ACodigoObra', editable: true, align: 'center', search: false, edittype: 'checkbox', width: 160, formatter: 'checkbox', hidden: true,
                formoptions: { rowpos: 10, colpos: 2 },
                editoptions: {
                    edithidden: true,
                    value: "True:False",
                    dataInit: function (e) {
                        e.style.width = '13px';
                        e.style.textAlign = 'center';
                    }
                },
                editrules: { edithidden: true }
            },
            {
                name: 'ActividadId', index: 'ActividadId', editable: true, edittype: 'select', hidden: true,
                formoptions: { rowpos: 11, colpos: 1 },
                editoptions: {
                    edithidden: true,
                    async: false,
                    dataUrl: "Actividades/GetSelect",
                    value: "",
                    maxlength: 100,
                    dataInit: function (e) {
                        e.style.width = '250px';

                    },
                    buildSelect: function (response) {
                        //var data = typeof response === "string" ? JSON.stringify(response) : response;
                        var data = JSON.parse(response);
                        s = "<select>";

                        $.each(data, function (i, item) {
                            s += '<option value="' + item.Id + '">' + item.Texto + '</option>';
                        })

                        return s + "</select>";
                    }
                },
                editrules: { edithidden: true }
            },
            {
                name: 'SubActividadId', index: 'SubActividadId', editable: true, edittype: 'select', hidden: true,
                formoptions: { rowpos: 11, colpos: 2 },
                editoptions: {
                    edithidden: true,
                    async: false,
                    dataUrl: "SubActividades/GetSelect",
                    value: "",
                    maxlength: 100,
                    dataInit: function (e) {
                        e.style.width = '250px';

                    },
                    buildSelect: function (response) {
                        //var data = typeof response === "string" ? JSON.stringify(response) : response;
                        var data = JSON.parse(response);
                        s = "<select>";

                        $.each(data, function (i, item) {
                            s += '<option value="' + item.Id + '">' + item.Texto + '</option>';
                        })

                        return s + "</select>";
                    }
                },
                editrules: { edithidden: true }
            },
            {
                name: 'DelegacionId', index: 'DelegacionId', editable: true, edittype: 'select', hidden: true,
                formoptions: { rowpos: 12, colpos: 1 },
                editoptions: {
                    edithidden: true,
                    async: false,
                    dataUrl: "Delegaciones/GetSelect",
                    value: "",
                    maxlength: 100,
                    dataInit: function (e) {
                        e.style.width = '250px';

                    },
                    buildSelect: function (response) {
                        //var data = typeof response === "string" ? JSON.stringify(response) : response;
                        var data = JSON.parse(response);
                        s = "<select>";

                        $.each(data, function (i, item) {
                            s += '<option value="' + item.Id + '">' + item.Texto + '</option>';
                        })

                        return s + "</select>";
                    }
                },
                editrules: { edithidden: true }
            },
            {
                name: 'ClienteId', index: 'ClienteId', editable: true, edittype: 'select', hidden: true,
                formoptions: { rowpos: 12, colpos: 2 },
                editoptions: {
                    edithidden: true,
                    async: false,
                    dataUrl: "Clientes/GetSelectClientes",
                    value: "",
                    maxlength: 100,
                    dataInit: function (e) {
                        e.style.width = '250px';

                    },
                    buildSelect: function (response) {
                        //var data = typeof response === "string" ? JSON.stringify(response) : response;
                        var data = JSON.parse(response);
                        s = "<select>";

                        $.each(data, function (i, item) {
                            s += '<option value="' + item.Id + '">' + item.Texto + '</option>';
                        })

                        return s + "</select>";
                    }
                },
                editrules: { edithidden: true }
            },
			{
			    name: 'FechaAlta', key: false, index: 'FechaAlta', editable: false, hidden: true
			}];


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
            name: 'Nueva Obra',
            iconClass: 'glyphicon glyphicon-plus',
            onClick: function () {
                grid.editGridRow("new", AddGridRow); 
            }
        },
        Modificar: {
            name: 'Modificar Obra',
            iconClass: 'glyphicon glyphicon-edit',
            onClick: function (row) {
                var rowKey = grid.getGridParam("selrow");
                grid.editGridRow(rowKey, EditGrid);
            },
            isEnabled: function(row)
            {
                return $(row).css("background-color") != "rgb(127, 255, 212)"; // color => aquamarine
            }
        },
        Borrar: {
            name: 'Borrar Obra',
            iconClass: 'glyphicon glyphicon-trash',
            onClick: function () {
                var rowKey = grid.getGridParam("selrow");
                grid.delGridRow(rowKey, DeleteRow);
            }
        },
        Buscar: {
            name: 'Buscar',
            iconClass: 'glyphicon glyphicon-search',
            onClick: function () {
                grid.searchGrid(Buscar);// delGridRow(rowKey, Delete);
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




