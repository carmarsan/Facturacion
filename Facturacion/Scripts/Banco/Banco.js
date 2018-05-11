var grid = $("#grid");
$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';

$(function () {


	grid.jqGrid({
		//url: '@Url.Action("GetAll", "Bancos")',
		url: 'Bancos/GetAll', 
		datatype: 'json',
		mtype: 'GET',
		contentType: "application/json; charset-utf-8",
		colNames: colnames,
		colModel: colmodel,
		pager: jQuery('#pager'),
		// Estas dos opciones son para que salga el listado con todos los registros y cada vez que se haga scroll pida los datos al servidor
		//page: 1,
		//scroll: 1,
		rowNum: 16,
		//loadui: "Bootstrap",
		styleUI: 'Bootstrap',
		rowList: [10, 20, 30, 40],
		height: 600,
		width: $("#divGrid").width, // 1100,
		caption: 'Listado de Bancos',
		sortname: 'Codigo',
		sortorder: "Asc",
		emptyrecords: 'No existen registros',
		ShrinkToFit: true,  // Para que ajuste las columnas automáticamente
		autowidth: true,
		viewrecords: true,
		loadonce: false,    // Sólo carga una vez todos los datos.  
		multiselect: false,
		loadError: function (xhr, status, err) {
		    alert(xhr);
		},
	    rowattr: function (rd) { 
	        return { 'class': 'hasSubmenu' }
	}
	});
	
	grid.jqGrid('navGrid', '#pager',{position: "left", cloneToTop: false}, Edit, Add, Delete, Buscar);

	grid.jqGrid('setLabel', 'Mostrar', '', { 'text-align': 'center' });
	grid.jqGrid('bindKeys');

});

var Edit = {
    // edit options
    zIndex: 100,
    url: 'Bancos/EditBancos',
    closeOnEscape: true,
    checkOnSubmit: true,
    closeAfterEdit: true,
    modal: true,
    ShrinkToFit: true,
    savekey: [true, 13],
    width: 700,
    recreateForm: true,
    afterComplete: function (response) {
        dialogo(response);
    },
    afterclickPgButtons: function () {
        setEditDialogCapture("Edición del banco", "Edit", grid, -1);
    },
    beforeShowForm: function () {
        setEditDialogCapture("Edición del banco", "Edit", grid, -1);
        centerDialog(grid);
    }
},
Add = {
    // add options
    zIndex: 100,
    url: "Bancos/AddBancos",
    closeOnEscape: true,
    savekey: [true, 13],
    width: 700,
    modal: true,
    closeAfterAdd: true,
    reloadAfterSubmit: true,
    drag: true,
    afterComplete: function (response) {
        dialogo(response);
    },
    beforeShowForm: function (form) {
        $('#Mostrar', form).prop("checked", true);
        setEditDialogCapture("Añadir banco", "Edit", grid, -1);

        centerDialog(grid);
    },
    afterclickPgButtons: function () {
        setEditDialogCapture("Añadir banco", "Edit", grid, -1);
    },
},
Buscar = {
    showQuery: false,
    closeOnEscape: false,
    //recreateForm: true,
    searchOnEnter: true,
    defaultSearch: "cn"
},
Delete = {
    // delete options
    zIndex: 100,
    url: "Bancos/DeleteBancos",
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
    beforeShowForm: function () {
        centerDialog(grid);
    }

};

/*
 *  SubMenu en el grid
 */
var submenu = new BootstrapMenu(".hasSubmenu",
{
    fetchElementData: function ($rowElem) {
        return $rowElem[0];
    },
    actionsGroups: [
        ['Nueva', 'Modificar'], ['Borrar'], ['Buscar'], ['Imprimir']
    ],
    actions: {
        Nueva: {
            name: 'Nuevo Banco',
            iconClass: 'glyphicon glyphicon-plus',
            onClick: function () {
                //var grid = $("#grid");
                grid.editGridRow("new", Add);
            }
        },
        Modificar: {
            name: 'Modificar Banco',
            iconClass: 'glyphicon glyphicon-edit',
            onClick: function (row) {
                var rowKey = grid.getGridParam("selrow");
                grid.editGridRow(rowKey, Edit);
            }
        },
        Borrar: {
            name: 'Borrar Banco',
            iconClass: 'glyphicon glyphicon-trash',
            onClick: function () {
                var rowKey = grid.getGridParam("selrow");
                grid.delGridRow(rowKey, Delete);
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


var colnames = ['BancoId', 'Código', 'Nombre', 'Dirección', 'Ver', 'Fecha Alta'],
    colmodel = [
			{ name: 'BancoId', key: true, hidden: true, index: 'BancoId', editable: true },
			{
			    name: 'Codigo',
			    key: false,
			    index: 'Codigo',
			    editable: true,
			    sortable: true,
			    firstsortorder: 'asc',
			    formoptions: { elmsuffix: ' *' },
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
			    name: 'Nombre',
			    key: false,
			    index: 'Nombre',
			    editable: true,
			    sortable: true,
			    width: 350,
			    resizable: true,
			    formoptions: { elmsuffix: ' *' },
			    editoptions: {
			        //size: 50,
			        maxlength: 50,
			        dataInit: function (e) {
			            e.style.width = '500px';

			        }
			    },
			    editrules: {
			        required: true
			    }
			},
			{
			    label: 'Dirección',
			    name: 'Direccion',
			    key: false,
			    index: 'Direccion',
			    editable: true,
			    width: 379,
			    editoptions: {
			        //size: 50,
			        maxlength: 200,
			        dataInit: function (e) {
			            e.style.width = '580px';

			        }
			    }
			},
			{
			    name: 'Mostrar',
			    index: 'Mostrar',
			    editable: true,
			    align: 'center',
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
			{ key: false, name: 'FechaAlta', index: 'FechaAlta', editable: false, hidden: true }];

