$(document).ready(function () {

	// Parámetros globales del grid
	var grid = $("#grid");
	$.jgrid.defaults.responsive = true;
	$.jgrid.defaults.styleUI = 'Bootstrap';

	var EditGrid = {
		url: 'Delegaciones/EditDelegacion',
		closeOnEscape: true,
		checkOnSubmit: true,
		closeAfterEdit: true,
		drag: true,
		modal: true,
		caption: "Edición",
		ShrinkToFit: true,
		savekey: [true, 13],
		width: 400,
		recreateForm: true,
		beforeShowForm: function () {
			setEditDialogCapture("Edición de la delegación", "Edit", grid);
			centerDialog(grid);
		},
		afterclickPgButtons: function () {
			setEditDialogCapture("Edición de la delegación", "Edit", grid);
		},
		afterComplete: function (response) {
			dialogo(response);
		}
	},
	AddGridRow = {
		zIndex: 100,
		width: 400,
		url: "Delegaciones/AddDelegacion",
		closeOnEscape: true,
		savekey: [true, 13],
		modal: true,
		closeAfterAdd: true,
		reloadAfterSubmit: true,
		editCaption: "Nuevo cliente",
		drag: true,
		beforeShowForm: function (form) {
			$('#Mostrar', form).prop("checked", true);
			setEditDialogCapture("Nuevo delegación", "Add", grid);

			centerDialog(grid);
		},
		afterComplete: function (response) {
			dialogo(response);
		}
	},
	Buscar = {
		showQuery: false,
		closeOnEscape: false,
		recreateForm: true,
		searchOnEnter: true,
		defaultSearch: "cn"
	},
	DeleteRow = {
		zIndex: 100,
		url: "Delegaciones/DeleteDelegacion",
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
			setEditDialogCapture("Borrar cliente", "Del", grid);
			centerDialog(grid);
		},
		afterComplete: function (response) {
			dialogo(response);
		}
	}

	grid.jqGrid({
		url: "Delegaciones/GetAll",
		datatype: 'json',
		mtype: 'Get',
		colNames: ['Id', 'Nombre', 'Abreviatura', 'Mostrar', 'Fecha Alta'],
		colModel: [
			{ key: true, hidden: true, name: 'DelegacionId', index: 'DelegacionId', editable: true },
			{
				key: false, name: 'NombreDelegacion', index: 'NombreDelegacion', editable: true, sortable: true, firstsortorder: 'asc', width: 130,
				editoptions: {
					maxlength: 150
				}
			},
			{
				key: false, name: 'Abreviatura', index: 'Abreviatura', editable: true, width: 50,
				editoptions: {
					dataInit: function (e) {
						e.style.width = '60px';
					},
					maxlength: 5,
				}
			},
			{
				key: false, name: 'Mostrar', index: 'Mostrar', editable: true, align: 'center', width: 35, edittype: 'checkbox',
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
		//autowidth: true,
		rowNum: 20,
		rowList: [10, 20, 30, 40],
		height: 600,
		width: 696,
		viewrecords: true,
		caption: 'Listado de Delegaciones',
		sortname: 'NombreDelegacion',
		sortorder: "Asc",
		emptyrecords: 'No existen registros',
		ShrinkToFit: true,  // Para que ajuste las columnas automáticamente
		headertitles: true,
		sortIconsBeforeText: true,
		iconSet: "fontAwesome",
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

	grid.jqGrid('navGrid', '#pager', { }, EditGrid, AddGridRow, DeleteRow, Buscar);

});