$(document).ready(function () {

	// SELECT del cliente.
	var $selClientes = $("#selectClientes");

	$.jgrid.defaults.responsive = true;
	$.jgrid.defaults.styleUI = 'Bootstrap';



    //Definición de columnas y modelos de datos
	var colNamesFacturas = ['FacturaId', 'Orden', 'Fac.Num', 'Fecha', 'Base Impo.', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', ''],
	ColModelFacturas =
			[
				{ name: 'FacturaId', index: 'FacturaId', hidden: true },
				{ name: 'Orden', index: 'Orden', hidden: true },
				{ name: 'NumeroFactura', index: 'NumeroFactura' /*,width: 60 */ },
				{ name: 'FechaFactura', index: 'FechaFactura'/*, width: 60 */ },
				{
					name: 'BaseImponible', index: 'BaseImponible', /*width: 70, */align: 'right', formatter: 'currency',
					formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, suffix: ' €' }
				},
				{ name: 'Tipo', index: 'Tipo', hidden: true },
				{ name: 'ObraId', index: 'ObraId', hidden: true },
				{ name: 'RotuloGerencia', index: 'RotuloGerencia', hidden: true },
				{ name: 'PorcentajeBonificacion', index: 'PorcentajeBonificacion', hidden: true },
				{ name: 'PorcentajeRetencion', index: 'PorcentajeRetencion', hidden: true },
				{ name: 'Subtotal', index: 'Subtotal', hidden: true },
				{ name: 'Bonificacion', index: 'Bonificacion', hidden: true },
				{ name: 'Retencion', index: 'Retencion', hidden: true },
				{ name: 'Total', index: 'Total', hidden: true },
				{ name: 'TotalAOrigen', index: 'TotalAOrigen', hidden: true },
				{ name: 'CertificacionesAnteriores', index: 'CertificacionesAnteriores', hidden: true },
				{ name: 'Cerrada', index: 'Cerrada', hidden: true },
				{ name: 'FacturaEspecial', index: 'FacturaEspecial', hidden: true },
				{ name: 'Contabilizado', index: 'Contabilizado', hidden: true },
				{ name: 'TieneTotalPropio', index: 'TieneTotalPropio', hidden: true },
				{ name: 'Detalles', index: 'Detalles', hidden: true }

			],
	colNamesObras = ['Id', 'Obras', ''],
	colModelObras =
			[
					{ name: 'ObraId', key: true, index: 'ObraId', hidden: true },
					{ name: 'Nombre', index: 'Nombre', width: 350 },
                    { name: 'Acabada', index: 'Acabada', hidden: true }
			],
	colNamesDetalle = ['DetalleId', 'Orden', 'Articulo', 'Cantidad', 'Precio Unit.', 'B.Imponible', 'ArticuloAlternativo', 'PorcentajeIva', 'TextoIva', 'TotalIVA'],
	colModelDetalle =
			[
				{ name: 'DetalleId', index: 'DetalleId', hidden: true },
				{ name: 'Orden', index: 'Orden', hidden: true },
				{ name: 'Articulo', index: 'Articulo', width: 800 },
				{
					name: 'Cantidad', index: 'Cantidad', align: 'right', formatter: 'currency',
					formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2 }
				},
				{
					name: 'PrecioUnitario', index: 'PrecioUnitario', align: 'right', formatter: 'currency',
					formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, suffix: ' €' }
				},
				{
					name: 'Importe', index: 'Importe', align: 'right', formatter: 'currency',
					formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", decimalPlaces: 2, suffix: ' €' }
				},
				{ name: 'ArticuloAlternativo', index: 'ArticuloAlternativo', hidden: true },
				{ name: 'PorcentajeIva', index: 'PorcentajeIva', hidden: true },
				{ name: 'TextoIva', index: 'TextoIva', hidden: true },
				{ name: 'TotalIVA', index: 'TotalIVA', hidden: true }
			],
	colNamesIva = ['Base Iva', '% Iva', 'fdsfas', 'Cuota Iva'],
	colModelIva =
		[
				{
					name: '_BaseIva', index: '_BaseIva', align: 'right', width: 80, formatter: 'currency',
					formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", suffix: ' €' }
				},
				{ name: '_TextoIva', index: '_TextoIva', align: 'center', width: 80 },
				{ name: '_PorcentajeIva', index: '_PorcentajeIva', hidden: true },

				{
				    name: '_CuotaIva', index: '_CuotaIva', align: 'right', width: 80, formatter: 'currency',
				    formatoptions: { decimalSeparator: ",", thousandsSeparator: ".", suffix: ' €' }
				}
		],

	colNamesTotales = ['', ''],
	colModelTotales = [
			{},
			{}
		
	];

	colNamesNuevaFactura = ['Id', 'Articulo', 'Ctdad', 'Precio', '% Iva', 'B.Imponible'];
	colModelNuevaFactura = [];

	// Parámetros globales del grid
	var grid = $("#grid")
	gridDetalle = $("#gridDetalle"),
	gridIva = $("#gridTablaIvas")
	gridTotales = $("#gridTablaTotales");

	// Tenemos que llenar el combo de clientes.
	$selClientes.append(RellenaClientes);

	// Si cambia el cliente tenemos que rellenar sus obras.
	$selClientes.on("change", function () {
		grid.trigger('reloadGrid');
	});

    // Grid de Obras
	grid.jqGrid({
		//url: 'Obras/Get?id=' + id,
		url: 'Obras/GetByCliente',
		postData: {
			id: function () { return $selClientes.val(); }
		},
		datatype: 'json',
		mtype: 'GET',
		gridview: true,
		contentType: "application/json; charset-utf-8",
		colNames: colNamesObras,
		colModel: colModelObras,
		scroll: 1,
		page: 1,
		autowidth: false,
		rowNum: 20,
		height: $("#divIzquierdo").height() - 95,
		width: $("#divIzquierdo").width() - 17,
		viewrecords: true,
		caption : '',
		sortname: 'Nombre',
		sortorder: "Asc",
		emptyrecords: 'No existen registros',
		ShrinkToFit: true,  // Para que ajuste las columnas automáticamente
		headertitles: true,
		sortIconsBeforeText: true,
		iconSet: "fontAwesome",
		loadonce: false,    // Sólo carga una vez todos los datos.
		multiselect: false,
		subGrid: true,
		onSelectRow: function (rowid, stat, e) {   // cuando pulsamos sobre una fila, hacemos que expanda el subgrid
		    if (e.button != 0)
		        return false;
			$(this).jqGrid("toggleSubGridRow", rowid);
		},
		loadError: function (xhr, status, err) {
			alert(xhr);
		},
		rowattr: function (rd) { // Si es un Abono le añadimos la clase que da color al fondo de la fila
		    if (rd.Acabada === 'True')
		            return { 'class': 'isObraCerrada' };
		        //else
		        //    return { 'class': 'isObraCerrada' }
		},
		loadComplete: function (data) {
			// TODO:  Cuando se ha seleccionado una factura con datos y después se selecciona un cliente u obra que no tiene nada, el detalle de la factura anterior
			//          se mantiene.  Tenemos que quitarlo.  El siguiente código es de pruebas.  No funciona
			//if (data.records == 0)
			//    $.jgrid.gridUnload('gridDetalle');
		},
        // Antes de abrir un subgrid, contraemos otros posibles que estén abiertos
		subGridBeforeExpand: function(divid, row_id){
		    var expanded = $("td.sgexpanded", grid)[0];
		    if (expanded)
		        setTimeout(function () {
		            $(expanded).trigger("click");
		        }, 100);
		},
		subGridRowExpanded: function (subgrid_id, row_id) {
			var subgrid_table_id, pager_id;
			subgrid_table_id = subgrid_id + '_t';
			pager_id = "p_" + subgrid_table_id;
			//$('#' + subgrid_id)

			$('#' + subgrid_id).html('<table id="' + subgrid_table_id + '" class="scroll"></table><div id="' + pager_id + '" class="scroll"></div>');
			$('#' + subgrid_table_id).jqGrid({
				url: 'Facturas/GetFacturasDeObra?id=' + row_id,
				datatype: 'json',
				mtype: 'POST',
				colNames: colNamesFacturas,
				colModel: ColModelFacturas,
				rowNum: 20,
				pager: pager_id,
				sortname: 'Orden',
				sortorder: 'asc',
				autowidth: true,
				ShrinkToFit: true,
				viewrecords: false,
				scroll: 1,
				page: 1,
				footerrow: true,
				userDataOnFooter: true,
				rowattr: function (rd) { // Si es un Abono le añadimos la clase que da color al fondo de la fila
				    if (rd.Tipo === 'A')
				        return { 'class': 'hasSubmenu isAbono' }
				    else
				        return {'class': 'hasSubmenu'}
				},
				gridComplete: function() {
					var $grid = $('#' + subgrid_table_id);
					var colSum = $grid.jqGrid('getCol', 'BaseImponible', false, 'sum');
					$grid.jqGrid('footerData', 'set', { FechaFactura: 'Total:', BaseImponible: colSum });

					var firstRow = $grid.getDataIDs()[0];
					if (firstRow)
					    $grid.setSelection(firstRow, true);
					    //$grid.jqGrid("setSelection", firstRow,); //.trigger("click");
				},
				onSelectRow: function (rowid, status, e){
				    // Cuando seleccionamos una factura, mandamos al controlador los datos de la factura para que nos devuelva 
				    //      las líneas de la factura.
				    gridDetalle.jqGrid('setGridParam', { url: "Detalles/GetDetails?id=" + rowid, datatype: 'json' });
				    gridDetalle.trigger('reloadGrid');

				    var factura = $(this).jqGrid('getRowData', rowid)

				    // Ponemos los datos de la factura
				    SetDatosFactura(factura);

				    // Tenemos que mirar si tiene Certificaciones Anteriores.
				    // Si tiene, activamos el panel 'PanelCertificacionesAnteriores'
				    if (factura.CertificacionesAnteriores == '0' && factura.TotalAOrigen == '0') {
				        $("#PanelCertificacionesAnteriores").hide();
				    }
				    else {
				        $("#PanelCertificacionesAnteriores").show();
				        $("#spanTotalOrigen").text('Total a Origen:  ' + factura.TotalAOrigen);
				        $("#spanCertAnt").text(factura.CertificacionesAnteriores);

				        $("#idTablaIva").append('<tr><td> </tr></td>');
				    };

				    gridIva.jqGrid('setGridParam', { url: "Detalles/GetIvasDeFactura?id=" + rowid, datatype: 'json' });
				    gridIva.trigger("reloadGrid");

				    var prueba = formatStrintToNumber(factura.Detalles, 2, "€");

				    // Añadimos los valores en la tabla de totales
				    //      - Subtotal
				    //      - 2 líneas en blanco
				    //      - Base Imponible
				    //      - IVA
				    //      - 3 líneas en blanco
				    //      - TOTAL.
				    $("#subtotal").empty().append('<span>' + formatStrintToNumber(factura.Subtotal, 2, "€") + '</span');
				    $("#baseimponible").empty().append('<span>' + formatStrintToNumber(factura.BaseImponible, 2, "€") + '</span');
				    $("#iva").empty().append('<span>' + formatStrintToNumber(factura.Detalles, 2, "€") + '</span');
				    $("#total").empty().append('<span>' + formatStrintToNumber(factura.Total, 2, "€") + '</span');
				}
			});
		}
	});

	
	// Grid de detalle
	gridDetalle.jqGrid({
		url: null, //'Detalles/GetDetails',
		//datatype: 'json',
		mtype: 'GET',
		colNames: colNamesDetalle,
		colModel:colModelDetalle,
		rowNum: 20,
		sortname: 'Orden',
		sortorder: 'asc',
		emptyrecords: 'No existen registros',
		height: $("#divDerecho").height() * 0.55, //- 115,
		width: $("#divDerecho").width() - 25,
		autowidth: false,
		ShrinkToFit: false,
		viewrecords: true,
		scroll: 1,
		page: 1,

	});

	// Centramos los títulos del grid de Detalle
	gridDetalle.jqGrid("setLabel", "Cantidad", "", { "text-align": "center" });
	gridDetalle.jqGrid("setLabel", "PrecioUnitario", "", { "text-align": "center" });
	gridDetalle.jqGrid("setLabel", "Importe", "", { "text-align": "center" });

	gridDetalle.trigger('reloadGrid');

	/*
	 * 
	 *  IVA
	 * 
	 */
	// Grid del IVA en el subtotal de la factura
	gridIva.jqGrid({
		url: null, //'empty.json',
		mtype: 'GET',
		colNames: colNamesIva,
		colModel: colModelIva,  
		rowNum: 5,
		sortname: '_TextoIva',
		sortorder: 'asc',
		emptyrecords: 'No existen registros',
		height: 95,
		//height: 'auto',
        width: 400,
		//width: $("#divDerecho").width() - 55,
		autowidth: false,
		ShrinkToFit: true,
		viewrecords: false,
		//scroll: 1,
		//page: 1,
		footerrow: true,
		userDataOnFooter: true,
		gridComplete: function() {
			var $grid = gridIva;
			var colSum = $grid.jqGrid('getCol', '_CuotaIva', false, 'sum');
			$grid.jqGrid('footerData', 'set', { _TextoIva: 'Total:', _CuotaIva: colSum });
		}
	});

	gridIva.jqGrid("setLabel", "_BaseIva", "", { "text-align": "center" });
	gridIva.jqGrid("setLabel", "_TextoIva", "", { "text-align": "center" });
	gridIva.jqGrid("setLabel", "_CuotaIva", "", { "text-align": "center" });



	

});




//
// Rellena el combo de clientes.
//
var RellenaClientes = function () {
	var s;
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
			//s = "<select>";

			$.each(data, function (i, item) {
				if (i > 0)
					s += '<option value="' + item.Id + '">' + item.Texto + '</option>';
			})
			//s += "</select>";
		},

		error: function (XMLHttpRequest, textStatus, errorThrown) {
			debugger;
		}

	});
	return s;
};


/*
 * Ponemos los datos de la factura (cliente, forma de pago, etc
 */
var SetDatosFactura = function (factura)
{
	$.ajax({
		type: "GET",
		contentType: 'application/json; charset=utf-8',
		//data: "",
		async: false,
		url: "Facturas/GetDataFromFactura?id=" + factura.FacturaId,
		dataType: "json",
		success: function (response) {
			selectFP = response;
			
			$("#inputNumFac").val(factura.NumeroFactura);
			$("#inputFechaFac").val(factura.FechaFactura);
			$("#inputCodigoCliente").val(response.CodCliente);
			$("#inputNombreCliente").val(response.Cliente);
			$("#inputCodigoObra").val(response.CodObra);
			$("#inputNombreObra").val(response.Obra);
			$("#inputDelegcion").val(response.DAS);
			$("#inputObraPlus").val(response.ObraPlus);
			$("#inputTextoFac").val(response.TextoFac);

			$("#inputDireccion").val(response.DireccionCliente);
			$("#inputLocalidad").val(response.Localidad);
			$("#inputCP").val(response.CP);
			$("#inputProvincia").val(response.Provincia);
			$("#inputCIF").val(response.CIF);
			$("#InputFormaPago").val(response.FormaPago);

			$("#textObservaciones").val(response.Observaciones);
		},

		error: function (XMLHttpRequest, textStatus, errorThrown) {
			debugger;
		}

	});
}


/*
 * Menu en el grid de Facturas.
 */
var submenu = new BootstrapMenu(".hasSubmenu",
{
    fetchElementData: function ($rowElem) {
        return $rowElem[0];
    },
    actionsGroups: [
        ['NuevaFac', 'ModificarFac'], ['GenAbonoDes, GenAbonoTotoDes'], ['CopiarFac', 'CopiarFacAnteCli'], ['Borrar'], ['Imprimir'], ['seeObrasCerradas']
    ],
    actions: {
        NuevaFac: {
            name: 'Nueva Factura',
            iconClass: 'glyphicon glyphicon-plus',
            onClick: function (row) {
                
                SetNuevaFactura(row);
            }
        },
        ModificarFac: {
            name: 'Modificar Factura/Abono seleccionado',
            iconClass: 'glyphicon glyphicon-edit',
            onClick: function () {

            }
        },
        CopiarFac: {
            name: 'Copiar Factura/Abono',
            iconClass: 'glyphicon glyphicon-copy',
            onClick: function () {

            }
        },
        CopiarFacAnteCli: {
            name: 'Copiar Fac. cliente anterior',
            iconClass: 'glyphicon glyphicon-level-up',
            onClick: function () {

            }
        },
        Borrar: {
            name: 'Borrar Factura/Abono',
            iconClass: 'glyphicon glyphicon-trash',
            onClick: function () {

            }
        },
        GenAbonoDes: {
            name: 'Generar Abono Desglosado',
            iconClass: '',
            onClick: function () {

            },
            isEnabled: function(row){
                return row.cells[5].innerText == 'F';
            }
        },
        GenAbonoTotoDes: {
            name: 'Generar Abono Total o Parcial',
            iconClass: '',
            onClick: function () {

            },
            isEnabled: function (row) {
                return row.cells[5].innerText == 'F';
            }
        },
        Imprimir: {
            name: 'Imprimir',
            iconClass: 'glyphicon glyphicon-print',
            onClick: function () {

            }
        },
        seeObrasCerradas: {
            name: 'Ver obras cerradas',
            iconClass: 'glyphicon glyphicon-eye-open',
            onClick: function () {

            }
        }
    }
});


var SetNuevaFactura = function (row) {
    // Si la obra está cerrada no se puede hacer ninguna factura.
    if (row.cells[17].title == 'False')
        alert('No se puede generar una factura nueva para la obra seleccionada porque está cerrada');
    else
        alert("Has dado en nueva fac.");
    /*
     * Que hacemos ahora?
     * 
     * Podemos sacar un diálogo de bootstrap o de jquery.
     */
};