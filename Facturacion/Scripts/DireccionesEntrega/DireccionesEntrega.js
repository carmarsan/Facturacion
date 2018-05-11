$(document).ready(function () {

	var grid = $("#gridDireccion");
	$.jgrid.defaults.responsive = true;
	$.jgrid.defaults.styleUI = 'Bootstrap';

	var GetClientes = function () {
		var selectFP;
		$.ajax({
			type: "GET",
			contentType: 'application/json; charset=utf-8',
			data: "",
			async: false,
			url: "/Clientes/GetSelectClientes",
			dataType: "json",
			success: function (response) {
				selectFP = response;

				var data = typeof response === "string" ? JSON.stringify(response) : response;
				var s = "";

				$.each(data, function (i, item) {

					s += item.Id + ":" + item.Texto + ";"; // '<option value="' + item.Id + '">' + item.Texto + '</option>';
				})
				selectFP = s.substring(0, s.length - 1);
			},

			error: function (XMLHttpRequest, textStatus, errorThrown) {
				debugger;
			}

		});
		return selectFP;

	};

	var EditGrid = {
		url: 'DireccionesEntrega/EditDireccion',
		closeOnEscape: true,
		checkOnSubmit: true,
		closeAfterEdit: true,
		drag: true,
		modal: true,
		caption: "Edición",
		ShrinkToFit: true,
		savekey: [true, 13],
		width: 750,
		recreateForm: true,
		//afterShowForm: RellenaPoblacion,
		beforeShowForm: function () {
			setEditDialogCapture("Edición de la dirección", grid);
			 centerDialog(grid);
		},
		afterclickPgButtons: function () {
			setEditDialogCapture("Edición de la dirección", grid);
		},
		afterComplete: function (response) {
			dialogo(response);
		}
	},
	AddGridRow = {
		zIndex: 100,
		width: 750,
		url: "DireccionesEntrega/AddDireccion",
		closeOnEscape: true,
		savekey: [true, 13],
		modal: true,
		closeAfterAdd: true,
		recreateForm: true,
		reloadAfterSubmit: true,
		drag: true,
		beforeShowForm: function (form) {
			$('#Mostrar', form).prop("checked", true);
			setEditDialogCapture("Nueva dirección", grid);
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
		url: "DireccionesEntrega/DeleteDireccion",
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
		afterComplete: function (response) {
			dialogo(response);
		},
		beforeShowForm: function (form) {
			setEditDialogCapture("Borrar Dirección de entrega", "Add", grid);
			centerDialog(grid);
		}
	}

	grid.jqGrid({
		url: 'DireccionesEntrega/GetAll',
		datatype: 'json',
		mtype: 'GET',
		gridview: true,
		contentType: "application/json; charset-utf-8",
		colNames: ['Id', 'Dirección', 'Localidad', 'CP', 'Provincia', 'CIF', 'Persona Contacto', 'Cliente', 'Ver', 'Fecha Alta'],
		colModel: [
			{
				name: 'DireccionEntregaId', key: true, index: 'DireccionEntregaId', editable: true, sortable: true, search: false, width: 90,
				sorttype: 'number',
				editoptions: {
					readonly: true,
					dataInit: function (e) {
						e.style.width = '90px';

					}
				}
			},
			{
				name: 'Direccion', index: 'Direccion', editable: true, sortable: true, width: 750, resizable: true,
				sorttype: 'text',
				//searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
				editoptions: {
					dataInit: function (e) {
						e.style.width = '550px';

					}
				}
			},
			{
				name: 'Localidad', index: 'Localidad', editable: true, sortable: true, width: 450, resizable: true,
				//formoptions: { rowpos: 2, colpos: 1 },
				searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
				editoptions: {
					maxlength: 100,
					dataInit: function (e) {
						e.style.width = '250px';

					}
				}
			},
			{
				name: 'CP', index: 'CP', editable: true, sortable: true, width: 110, resizable: true,
				searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
				editoptions: {
					maxlength: 100,
					dataInit: function (e) {
						e.style.width = '90px';

					}
				}
			},
			{
				name: 'ProvinciaId', index: 'ProvinciaId', editable: true, sortable: true, width: 180, resizable: true, edittype: 'select',
				searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
				editoptions: {
					async: false,
					dataUrl : "Provincias/GetAll",
					value : "",
					maxlength: 100,
					//edithidden: true,
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
				}
			},
			{
				name: 'CIF', index: 'CIF', editable: true, sortable: true, width: 180, resizable: true,
				searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
				editoptions: {
					maxlength: 100,
					//edithidden: true,
					dataInit: function (e) {
						e.style.width = '100px';
					},
					dataEvents: [{
						type: 'blur',
						fn: function (e) {
							if (e.target != null) {
								if (!textCIF($(e.target).val()))
									//if (!idValidator.checkCIF($(e.target).val()))
									alert('CIF erróneo.  Por favor corríjalo antes de guardar');
							}
						}
					}]
				}
			},
			{
				name: 'PersonaContacto', index: 'PersonaContacto', editable: true, sortable: true, width: 150, resizable: true, hidden: true,
				//formoptions: { rowpos: 9, colpos: 2 },
				editoptions: {
					maxlength: 150,
					edithidden: true,
					dataInit: function (e) {
						e.style.width = '250px';

					}
				},
				editrules: {
					edithidden: true,
				}
			},
			{
				name: 'ClienteId', index: 'ClienteId', edittype: 'select', editable: true, sortable: true, width: 150,
				editoptions: {
					dataUrl: 'Clientes/GetSelectClientes',
					async: false,
					value: "", // GetClientes,
					buildSelect: function (response) {
						var data = JSON.parse(response);

						select = "<select>"; 

						$.each(data, function (i, item) {

							select += '<option value="' + item.Id + '">' + item.Texto + '</option>';
						})
						return select + "</select>";
					}
				}
			},
			{
				name: 'Mostrar', index: 'Mostrar', editable: true, align: 'center', search: false, edittype: 'checkbox', width: 100, formatter: 'checkbox',
				//formoptions: { rowpos: 11, colpos: 2 },
				editoptions: {
					value: "True:False",
					dataInit: function (e) {
						e.style.width = '13px';
						e.style.textAlign = 'center';
					}
				}
			},
			
			{ name: 'FechaAlta', key: false, index: 'FechaAlta', editable: true, hidden: true }],
		pager: jQuery('#pager'),
		// Estas dos opciones son para que salga el listado con todos los registros y cada vez que se haga scroll pida los datos al servidor
		//page: 1,
		//scroll: 1,
		autowidth: true,
		rowNum: 20,
		rowList: [10, 20, 30, 40],
		height: 700,
		width: 720,
		viewrecords: true,
		caption: 'Listado de Direcciones de Entrega',
		sortname: 'DireccionEntregaId',
		sortorder: "Asc",
		emptyrecords: 'No existen registros',
		ShrinkToFit: true,  // Para que ajuste las columnas automáticamente
		autowidth: true,
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

	grid.jqGrid('navGrid', '#pager', { /*search: true,  add: true, edit: true, del: true*/
		//refresh: true,
		//view: true,
		//position: "left",
		//cloneToTop: false
	}, EditGrid, AddGridRow, DeleteRow, Buscar);


});


function testCIF(cif) {
	var pares = 0;
	var impares = 0;
	var suma;
	var ultima;
	var unumero;
	var uletra = new Array("J", "A", "B", "C", "D", "E", "F", "G", "H", "I");
	var xxx;
	var texto = cif.toUpperCase();
	var regular = new RegExp(/^[ABCDEFGHJKLMNPQS]\d{7}[0-9,A-J]$/g);
	if (!regular.exec(texto)) {
		trace("1");
		return false;
	}
	ultima = texto.substr(8, 1);
	for (var cont = 1; cont < 7; cont++) {
		xxx = (2 * parseInt(texto.substr(cont++, 1))).toString() + "0";
		impares += parseInt(xxx.substr(0, 1)) + parseInt(xxx.substr(1, 1));
		pares += parseInt(texto.substr(cont, 1));
	}
	xxx = (2 * parseInt(texto.substr(cont, 1))).toString() + "0";
	impares += parseInt(xxx.substr(0, 1)) + parseInt(xxx.substr(1, 1));
	suma = (pares + impares).toString();
	unumero = parseInt(suma.substr(suma.length - 1, 1));
	unumero = (10 - unumero);
	if (unumero == 10) {
		unumero = 0;
	}
	if ((ultima == String(unumero)) || (ultima == uletra[unumero])) {
		return true;
	} else {
		return false;
	}
}