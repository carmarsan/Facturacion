// Parámetros globales del grid
var grid = $("#grid");
$.jgrid.defaults.responsive = true;
$.jgrid.defaults.styleUI = 'Bootstrap';


$(document).ready(function () {

	$("#closedialog").addClass("btn btn-primary");

	grid.jqGrid({
		url: 'Clientes/GetAll',
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
		//width: 920,
		viewrecords: true,
		caption: 'Listado de Clientes',
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
			alert(xhr);
		},
		loadComplete: function (data) {
		}
	});

	grid.jqGrid('navGrid', '#pager', { }, EditGrid, AddGridRow, DeleteRow, Buscar);

	grid.jqGrid('setLabel', 'Mostrar', '', { 'text-align': 'center' });
	grid.jqGrid('bindKeys');

});

var EditGrid = {
    url: 'Clientes/EditCliente',
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
    afterShowForm: RellenaPoblacion,
    beforeShowForm: function () {
        setEditDialogCapture("Edición del cliente", "Edit", grid);
        centerDialog(grid);
    },
    afterclickPgButtons: function () {
        setEditDialogCapture("Edición del cliente", "Edit", grid);
    },
    afterComplete: function (response) {
        dialogo(response);
    }
}, AddGridRow = {
    zIndex: 100,
    width: 1236,
    url: "Clientes/AddCliente",
    closeOnEscape: true,
    savekey: [true, 13],
    modal: true,
    closeAfterAdd: true,
    reloadAfterSubmit: true,
    editCaption: "Nuevo cliente",
    drag: true,
    beforeShowForm: function (form) {
        $('#Mostrar', form).prop("checked", true);
        setEditDialogCapture("Nuevo cliente", "Add", grid);

        centerDialog(grid);
    },
    afterComplete: function (response) {
        dialogo(response);
    }
}, Buscar = {
    //multipleSearch: false,
    //multipleGroup: false,
    showQuery: false,
    closeOnEscape: false,
    //recreateForm: true,
    searchOnEnter: true,
    defaultSearch: "cn"
}, DeleteRow = {
    zIndex: 100,
    url: "Clientes/DeleteCliente",
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

/*
 * Función para mostrar un cuadro de diálogo con las Direcciones de Entrega de un cliente
 * 
 */
var showDirecciones = function (id) {
	$('#gridDEDialog').dialog({
		//autoOpen: false,
		dragable: true,
		modal: true,
		width: 750,
		position: { my: "center", at: "center", of: $("body"), within: $("body") },
		//resizeable: true,
		buttons: {
			"Guardar": function () {
				var i, count, $grid = $("#gridDE"), rowSelected = {};
				var rowArray = $grid.jqGrid('getGridParam', 'selarrrow');
				if (rowArray != null && rowArray.length != 0)
				{
					// Tenemos que mandarlo a DireccionesEntregaController para que guarde en dicha tabla 
					// También tenemos que enviarle el id del cliente => id.
					rowSelected.IdsDirecciones = rowArray;
					rowSelected.IdCliente = id;

					$.ajax({
						type: 'POST',
						contentType: 'application/json; charset=utf-8',
						data: JSON.stringify(rowSelected),
						async: false,
						url: "DireccionesEntrega/UpdateClientDirections",
						dataType: "json",
						success: function (response) {
							select = "";
						},

						error: function (XMLHttpRequest, textStatus, errorThrown) {
							debugger;
						}
					})

				}
			},
			"Cerrar": function () {
				$(this).dialog('close');
			}
		},
		open: function () {
            // Reseteamos el grid para los nuevos datos
		    $.jgrid.gridUnload('gridDE')
			GetGrid_Data(id);
		},
		close: function () {
			$(this).dialog('close');
		}
	}).dialog({
	    position: {
	        my: "center center",
	        at: "center center",
	        of: window
	    }
	});
};

var RellenaPoblacion = function () {
    var sel_id = grid.jqGrid('getGridParam', 'selrow');
    var selRowData = grid.jqGrid('getRowData', sel_id);


    var poblacionId = grid.jqGrid('getRowData', grid[0].p.selrow).PoblacionId; //$("#PoblacionId").val();
    var provinciaId = $("#ProvinciaId").val();
    if (provinciaId != 0) {
        $("#PoblacionId")
                .html("<option value=''>Cargando poblaciones...</option>")
                .attr("disabled", "disabled");

        var poblaciones = GetPoblaciones(provinciaId);
        $("#PoblacionId").html(""); // Borramos lo que tenga el control

        $("#PoblacionId").append("<option data-cp=0 value='0'>Seleccione una Localidad</option>");

        $.each(poblaciones, function (i, city) {
            if (city.Texto == poblacionId)
                $("#PoblacionId").append($('<option data-cp=' + city.CP + ' selected></option>').val(city.Id).html(city.Texto));
            else
                $("#PoblacionId").append($('<option data-cp=' + city.CP + '></option>').val(city.Id).html(city.Texto));
        });

        $("#PoblacionId").removeAttr("disabled");


        //$("#PoblacionId option[text='" + poblacionId + "']").attr("selected", "selected");

        var seleccion = $('#PoblacionId option:selected')

        //.data("cp")

        $("#PoblacionId").val(seleccion.val());
    }
}

/*
 * Grid de las direcciones de Entrega del cliente.
 */
function GetGrid_Data(id) {
    $("#gridDE").jqGrid({
        url: 'DireccionesEntrega/GetAll',
        datatype: 'json',
        mtype: 'GET',
        gridview: true,
        contentType: "application/json; charset-utf-8",
        colNames: ['Id', 'Direccion', 'Localidad', 'ClienteId'],
        colModel: [
			{
			    name: 'DireccionEntregaId', key: true, index: 'DireccionEntregaId', editable: true, sortable: true, search: false, width: 90, sorttype: 'number',
			},
			{
			    name: 'Direccion', index: 'Direccion', editable: true, sortable: true, width: 750, resizable: true, sorttype: 'text',
			},
			{
			    name: 'Localidad', index: 'Localidad', editable: true, sortable: true, width: 450, resizable: true,
			},
			{
			    name: 'ClienteId', index: 'ClienteId', hidden: true,
			}
        ],
        pager: $('#pagerDE'),
        autowidth: true,
        height: 500,
        width: 720,
        caption: 'Listado de Direcciones de Entrega',
        sortname: 'DireccionEntregaId',
        sortorder: "Asc",
        emptyrecords: 'No existen registros',
        ShrinkToFit: true,  // Para que ajuste las columnas automáticamente
        viewrecords: true,
        headertitles: true,
        iconSet: "fontAwesome",
        loadonce: false,    // Sólo carga una vez todos los datos.
        multiselect: true,
        loadError: function (xhr, status, err) {
            alert(xhr);
        },
        loadComplete: function (data) {
            // Nos vamos recorriendo todas las filas para marcar las que tengan un idCliente = id
            if (data.rows.length > 0) {
                for (var i = 0; i < data.rows.length; i++) {
                    if (parseInt(data.rows[i].cell[9]) == id) {
                        jQuery("#gridDE").jqGrid('setSelection', data.rows[i].id, true);
                        // Deshabilitamos el checkbox
                        $("#jqg_gridDE_" + data.rows[i].id).prop("disabled", true);

                    }
                }
            }
        },
        beforeSelectRow: function (rowid, e) {
            // No dejamos que se desmarquen las que ya están marcadas
            if ($("#jqg_gridDE_" + rowid).prop("disabled") == true) {
                return false;
            }
            return true;
        },
    });
}

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

var GetFormasPago = function () {
    var selectFP;
    $.ajax({
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        data: "",
        async: false,
        url: "/Clientes/GetFormasPago",
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

},
// Select con las provincias
GetProvincias = function () {

    var select;
    $.ajax({
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        data: "",
        async: false,
        url: "/Provincias/GetAll",
        dataType: "json",
        success: function (response) {
            select = "";

            var data = typeof response === "string" ? JSON.stringify(response) : response;
            //var s = "0:Seleccione una Provincia;";
            var s = "";

            $.each(data, function (i, item) {

                s += item.Id + ":" + item.Texto + ";";
            })
            select = s.substring(0, s.length - 1);
        },

        error: function (XMLHttpRequest, textStatus, errorThrown) {
            debugger;
        }

    });
    return select;
},
GetPoblaciones = function (idProvincia) {
    var select;
    $.ajax({
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        data: "id=" + idProvincia,
        async: false,
        url: "/Poblaciones/GetSelect",
        dataType: "json",
        success: function (response) {
            select = typeof response === "string" ? JSON.stringify(response) : response;

            //var data = typeof response === "string" ? JSON.stringify(response) : response;
            ////var s = "0:Seleccione una Provincia;";
            //var s = "";

            //$.each(data, function (i, item) {

            //    s += item.Id + ":" + item.Texto + ";";
            //})
            //select = s.substring(0, s.length - 1);
        },

        error: function (XMLHttpRequest, textStatus, errorThrown) {
            debugger;
        }

    });
    return select;
};

/*
 * 
 *      Modelo
 * 
 * 
 */
var colnames = ['Id', 'Nombre', 'Titular', 'Domicilio', 'Provincia', 'Localidad', 'CP', 'CIF', 'Teléfono 1', 'Teléfono 2', 'Móvil 1', 'Móvil 2', 'Fax 1', 'Fax 2', 'Email', 'Web', 'Observaciones', 'Persona Contacto', 'Sello', 'Codigo Contable', 'Forma Pago', 'Ver', 'Fecha Alta', ''],
    colmodel = [
			{
			    name: 'ClienteId', key: true, index: 'ClienteId', editable: true, sortable: true, search: false, width: 90,
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
			    name: 'NombreCliente', index: 'NombreCliente', editable: true, sortable: true, width: 550, resizable: true,
			    formoptions: { rowpos: 1, colpos: 2 },
			    sorttype: 'text',
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
			    name: 'Titular', index: 'Titular', editable: true, sortable: true, width: 450, resizable: true,
			    formoptions: { rowpos: 2, colpos: 1 },
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
			    name: 'Domicilio', index: 'Domicilio', editable: true, sortable: true, width: 450, resizable: true,
			    formoptions: { rowpos: 2, colpos: 2 },
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
			    name: 'ProvinciaId', index: 'ProvinciaId', editable: true, search: false, sortable: true, width: 250, resizable: true, edittype: 'select',
			    formoptions: { rowpos: 3, colpos: 1 },
			    searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'], value: GetProvincias },
			    editoptions: {
			        value: GetProvincias,
			        maxlength: 100,
			        //edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '250px';
			        },
			        dataEvents: [
						{
						    type: 'change',
						    fn: function (e) {
						        var v = $(e.target).val();
						        var poblaciones = GetPoblaciones(v);
						        $("#PoblacionId").html(""); // Borramos lo que tenga el control

						        $.each(poblaciones, function (i, city) {
						            $("#PoblacionId").append(
										($('<option></option>').val(city.Id).html(city.Texto))).attr("data-cp", city.CP);
						        });
						        $("#CP").val(poblaciones[0].CP);
						    }
						}
			        ]
			    }
			},
			{
			    name: 'PoblacionId', index: 'PoblacionId', editable: true, sortable: true, width: 350, resizable: true, edittype: 'select',
			    formoptions: { rowpos: 3, colpos: 2 },
			    searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
			    editoptions: {
			        //readonly: true,
			        value: "0:", //GetPoblaciones2,
			        maxlength: 100,
			        //edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '250px';
			        },
			        dataEvents: [
						{
						    type: 'change', // Si cambia la población tenemos que ponerle el CP
						    fn: function (e) {
						        var v = $(e.target).val();

						        //var poblaciones = GetPoblaciones(v);
						        $("#CP").val(""); // Borramos lo que tenga el control

						        $.ajax({
						            url: '/Poblaciones/GetCP',
						            type: "GET",
						            dataType: "JSON",
						            async: false,
						            data: { Id: v },
						            success: function (value) {
						                //var cp = JSON.stringify(value);
						                $("#CP").val(value);
						            }
						        });

						    }
						}
			        ]
			    }
			},
			{
			    name: 'CP', index: 'CP', editable: true, sortable: true, width: 110, resizable: true,
			    formoptions: { rowpos: 4, colpos: 1 },
			    searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
			    editoptions: {
			        readonly: true,
			        maxlength: 100,
			        //edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '90px';

			        }
			    }
			},

			{
			    name: 'CIF', index: 'CIF', editable: true, sortable: true, width: 180, resizable: true,
			    formoptions: { rowpos: 4, colpos: 2 },
			    searchoptions: { sopt: ['eq', 'ne', 'bw', 'cn'] },
			    editoptions: {
			        maxlength: 100,
			        //edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '180px';
			        },
			        dataEvents: [{
			            type: 'blur',
			            fn: function (e) {
			                if (!textCIF($(e.target).val()))
			                    //if (!idValidator.checkCIF($(e.target).val()))
			                    alert('CIF erróneo.  Por favor corríjalo antes de guardar');
			            }
			        }]
			    }
			},
			{
			    name: 'Telefono1', index: 'Telefono1', editable: true, sortable: true, width: 160, resizable: true,
			    formoptions: { rowpos: 5, colpos: 1 },
			    editoptions: {
			        maxlength: 100,
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '90px';

			        }
			    }
			},
			{
			    name: 'Telefono2', index: 'Telefono2', editable: true, sortable: true, width: 160, resizable: true, hidden: true,
			    formoptions: { rowpos: 5, colpos: 2 },
			    editoptions: {
			        //maxlength: 100,

			        dataInit: function (e) {
			            e.style.width = '90px';

			        }
			    },
			    editrules: {
			        edithidden: true,
			    }

			},
			{
			    name: 'Movil1', index: 'Movil1', editable: true, sortable: true, width: 160, resizable: true,
			    formoptions: { rowpos: 6, colpos: 1 },
			    editoptions: {
			        maxlength: 100,
			        //edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '90px';

			        }
			    }
			},
			{
			    name: 'Movil2', index: 'Movil2', editable: true, sortable: true, width: 160, resizable: true, hidden: true,
			    formoptions: { rowpos: 6, colpos: 2 },
			    editoptions: {
			        maxlength: 100,
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '90px';

			        }
			    },
			    editrules: {
			        edithidden: true,
			    }
			},
			{
			    name: 'Fax1', index: 'Fax1', editable: true, sortable: true, width: 160, resizable: true, hidden: true,
			    formoptions: { rowpos: 7, colpos: 1 },
			    editoptions: {
			        maxlength: 100,
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '90px';

			        }
			    },
			    editrules: {
			        edithidden: true,
			    }
			},
			{
			    name: 'Fax2', index: 'Fax2', editable: true, sortable: true, width: 160, resizable: true, hidden: true,
			    formoptions: { rowpos: 7, colpos: 2 },
			    editoptions: {
			        maxlength: 100,
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '90px';

			        }
			    },
			    editrules: {
			        edithidden: true,
			    }
			},
			{
			    name: 'Email', index: 'Email', editable: true, sortable: true, width: 150, resizable: true,
			    formoptions: { rowpos: 8, colpos: 1 },
			    formatter: 'email',
			    editoptions: {
			        maxlength: 100,
			        //edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '150px';

			        }
			    },
			    editrules: {
			        email: true,
			        required: true
			    }
			},
			{
			    name: 'Web', index: 'Web', editable: true, sortable: true, width: 150, resizable: true, hidden: true,
			    formoptions: { rowpos: 8, colpos: 2 },
			    editoptions: {
			        maxlength: 100,
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '250px';

			        }
			    },
			    editrules: {
			        url: true,
			        edithidden: true,
			    }
			},
			{
			    name: 'Observaciones', index: 'Observaciones', edittype: 'textaera', editable: true, sortable: true, width: 150, resizable: true, hidden: true,
			    formoptions: { rowpos: 9, colpos: 1 },
			    editoptions: {
			        rows: "2", cols: "50",
			        maxlength: 100,
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
			    name: 'PersonaContacto', index: 'PersonaContacto', editable: true, sortable: true, width: 150, resizable: true, hidden: true,
			    formoptions: { rowpos: 9, colpos: 2 },
			    editoptions: {
			        maxlength: 100,
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
			    name: 'Sello', index: 'Sello', editable: true, align: 'center', search: false, edittype: 'checkbox', width: 69, formatter: 'checkbox', hidden: true,
			    formoptions: { rowpos: 10, colpos: 1 },
			    editoptions: {
			        value: "True:False",
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '13px';
			            e.style.textAlign = 'center';
			        }
			    },
			    editrules: {
			        edithidden: true,
			    }
			},
			{
			    name: 'CodigoContable', index: 'CodigoContable', editable: true, sortable: true, width: 150, resizable: true, hidden: true,
			    formoptions: { rowpos: 10, colpos: 2 },
			    editoptions: {
			        maxlength: 100,
			        edithidden: true,
			        dataInit: function (e) {
			            e.style.width = '200px';

			        }
			    },
			    editrules: {
			        edithidden: true,
			    }
			},
			{
			    name: 'FormaPagoId', index: 'FormaPago', editable: true, edittype: "select", width: 150, resizable: true, hidden: true,
			    formoptions: { rowpos: 11, colpos: 1 },
			    editoptions: {
			        value: GetFormasPago,
			        //maxlength: 100,
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
			    name: 'Mostrar', index: 'Mostrar', editable: true, align: 'center', search: false, edittype: 'checkbox', width: 160, formatter: 'checkbox',
			    formoptions: { rowpos: 11, colpos: 2 },
			    editoptions: {
			        value: "True:False",
			        dataInit: function (e) {
			            e.style.width = '13px';
			            e.style.textAlign = 'center';
			        }
			    }
			},
			{
			    name: 'FechaAlta', key: false, index: 'FechaAlta', editable: false, hidden: true
			},
			{
			    name: "Boton", search: false, formatter: function (cellvalue, option, rowobject) {
			        return "<button type='button' id='showDireccion" + option.rowId + "' title='Mosrar Direcciones de Entrega' class='btn btn-primary btn-xs glyphicon glyphicon-road' onclick='showDirecciones(" + option.rowId + ");'></button>";
			    },
			    editoptions: {
			        dataInit: function (e) {
			            e.style.textAlign = 'center';
			        }
			    }
			}];

