﻿
var commerce = commerce || {};
commerce.amazon = commerce.amazon || {};
commerce.amazon.web = commerce.amazon.web || {};

//Declare a class, with parameteres
commerce.amazon.web.users =
	(function () {
		var MyAuxClass = function () {
			var that = this;


			this.Init = function () {
				that.FindUsuarios();
			};

			//----------------------events------------------------//

			$('#idbtnRegisterUsuario').click(function () {

				that.RegistrarUsuario();

			});

			$('#idOpenModalUser').click(function () {
				that.LoadUser({}, false)
			});

			//----------------------End event------------------------//

			//----------------------AJAX------------------------//
            
            this.RegistrarUsuario = function () {
                $("#errorMsgDiv").html('');
                var form = document.getElementById("formUsuario");
                $.validator.unobtrusive.parse(form)
                
                if ($(form).valid()) {
                    var radios = document.getElementsByName('estado');
                    var checkedBtn;
                    for (var i = 0; i < radios.length; i++) {
                        if (radios[i].checked) {
                            checkedBtn = parseInt(radios[i].value);
                            break;
                        }
                    }

                    var User = {};
                    User.Role = checkedBtn;
                    if (!User.Role) {
                        $("#errorMsgDiv").html('Por favor Seleccionar Role');
                        return;
                    }
                    that.LastAjaxCall = $.ajax({
                        type: "POST",
                        url: "/Account/Register",
                        data: User,
						success: function (data) {
							//console.log(data);
							if (HandleResponse(data)) {

								if (data && data.Status === 0) {
									//var idUsuario = data.Account.Result.UserId
									$('.modal button.myclose').click();
									that.FindUsuarios();
								} else {
									$("#errorMsgDiv").html('<span class="error">' + data.Message + '</span>')
								}
							} else {
								console.log("Error al obtener los valores");
							}
						},
						error: function (err) {
							console.log("Error al obtener los valores");
						}
					})
				}
            }

			this.FindUsuarios = function () {
				// var HasEstablecimiento = $('#idEstab').val()
				that.LastAjaxCall = $.ajax({
					type: "POST",
					url: "/Account/FindUsuarios",
					data: {},
					success: function (data) {
						if (HandleResponse(data)) {

							that.LoadUsers(data);
						} else {
							console.log("Error al obtener los valores");
						}
					},
					error: function (err) {
						console.log("Error al obtener los valores");
					}
				})
			}

			this.LoadUser = function (user, disable = true) {
                //console.log(user);
                var form = document.getElementById("formUsuario");
                $('#formUsuario .field-validation-error').text('');
                if (disable) {
                    $('#idbtnRegisterUsuario').attr('disabled', true);
                } else {
                    $('#idbtnRegisterUsuario').removeAttr('disabled');
                }

				$('#Nom').val(user.Nom);
				$('#Prenom').val(user.Prenom);
				$('#Email').val(user.Email);
				$('#UserId').val(user.UserId);

				var radios = document.getElementsByName('roleUser');
                for (var i = 0; i < radios.length; i++) {
                    if (radios[i].value == user.Role) {
                        radios[i].checked = true;
                    } else {
                        radios[i].checked = false;
                    }
                }
            }

			this.LoadUsers = function (data) {

				var columns = [{
					data: 'Foto',
					render: function (data) {
						if (!data) {
							data = 'default pic.jpg'
						}
						var img = "<img class='avatar img-circle img-thumbnail' src='/images/" + data + "'>"
						//console.log(img)
						return img
					}
				},
				{
                    data: 'UserId',
                    render: function (data, type, row) {
                        return "<a style='cursor: pointer; text-decoration: underline' class='cursorPointer loadUsuario' data-toggle='modal' data-target='#myModalUsuario' data-id='" + row.UserId + "'>" + data + "</a>";
                    }

				}, {
					data: 'Nombre',

				}, {
					data: 'Apellidos'
				},
				{
					data: 'TelUsuario'
				},
				{
					data: 'Email'
				},
				{
					data: 'RoleName',
				}
				]
				that.DetailUsuario = $('#TabUsuario').DataTable({
					responsive: true,
					data: data,
					pageLength: 15,
					destroy: true,
					//scrollX: true,
					dom: "<'col-sm-12 col-md-6 pull-left'l><'col-sm-12 col-md-6 pull-right'f>rt<'col-sm-12 col-md-6 pull-left'i><'col-sm-12 col-md-6 pull-right'p>",
					columns: columns,
					language: {
						processing: "Tratamiento en curso...",
						search: "Buscar&nbsp;:",
                        lengthMenu: "Elementos por página _MENU_",
						info: "",
						infoEmpty: "",
						infoFiltered: "(Filtrado con _MAX_ usuarios en total)",
						infoPostFix: "",
						loadingRecords: "Cargando...",
						zeroRecords: "No hay elementos para mostrar",
						emptyTable: "No hay datos disponibles en la tabla",
						paginate: {
							first: "Primera",
							previous: "Anterior",
							next: "Siguiente",
							last: "Último"
						},
						aria: {
							sortAscending: ": Activar para ordenar la columna en orden ascendente",
							sortDescending: ": Activar para ordenar la columna en orden descendente"
						}
                    },
                    drawCallback: function (settings) {
                        $(".loadUsuario").on("click", function () {
                            var userId = $(this).data("id");
                            //console.log(userId);
                            $.each(data, function (i, user) {
                                if (user.UserId === userId) {
                                    that.LoadUsuario(user);
                                }
                            })
                            //$(".modal-body #txtId").val($(this).data("id"));

                        });
                    }
				});

				$($.fn.dataTable.tables()).DataTable().columns.adjust();

			}

			//----------------------end function------------------------//

		}

		return new MyAuxClass();
    })();


// IIFE - Immediately Invoked Function Expression
(function ($, window, document) {

	// The $ is now locally scoped
	// Listen for the jQuery ready event on the document
	$(function () {
		//Document Ready Actions

		Commerce.Amazon.Web.users.Init();
	});
	// The rest of the code goes here!
}(window.jQuery, window, document));
