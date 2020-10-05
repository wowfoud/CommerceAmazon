
var commerce = commerce || {};
commerce.amazon = commerce.amazon || {};
commerce.amazon.web = commerce.amazon.web || {};

//Declare a class, with parameteres
commerce.amazon.web.users =
    (function () {
        var MyAuxClass = function () {
            var that = this;


            this.Init = function () {
                that.FindUsers();
            };

            //----------------------events------------------------//

            $('#idBtnSaveUser').click(function () {

                that.SaveUser();

            });

            //$('#idOpenModalUser').click(function () {
            //    that.LoadUser({}, false)
            //});

            //----------------------End event------------------------//

            //----------------------AJAX------------------------//

            this.SaveUser = function () {
                $("#errorMsgDiv").html('');
                var form = document.getElementById("formUsuario");
                $.validator.unobtrusive.parse(form)
                if ($(form).valid()) {
                    var radios = document.getElementsByName('roleUser');
                    var idRole;
                    for (var i = 0; i < radios.length; i++) {
                        if (radios[i].checked) {
                            idRole = parseInt(radios[i].value);
                            break;
                        }
                    }
                    var idUser = !!that.User ? that.User.Id : 0;
                    var user = {
                        Id: idUser,
                        Nom: $('#Nom').val(),
                        Prenom: $('#Prenom').val(),
                        Email: $('#Email').val(),
                        UserId: $('#UserId').val(),
                        Role: idRole
                    };
                    if (!user.Role) {
                        $("#errorMsgDiv").html('SVP, Selectioner le role');
                        return;
                    }
                    $.ajax({
                        type: "POST",
                        url: "/User/SaveUser",
                        data: user,
                        success: function (data) {
                            //console.log(data);
                            if (HandleResponse(data)) {

                                if (data && data.Status === 0) {
                                    $('.modal button.myclose').click();
                                    that.FindUsers();
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
                    });
                }
            }

            this.FindUsers = function () {
                $.ajax({
                    type: "POST",
                    url: "/User/FindUsers",
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
                that.User = user;
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
                //$('#myModalUsuario').modal('show');
            }

            this.LoadUsers = function (users) {

                var columns = [
                    {
                        data: 'Id',
                        render: function (data, type, row) {
                            return "<a style='cursor: pointer; text-decoration: underline' class='cursorPointer loadUser' data-toggle='modal' data-target='#myModalUsuario' data-id='" + data + "'>" + data + "</a>";
                        }
                    },
                    {
                        data: 'Nom',

                    },
                    {
                        data: 'Prenom'
                    },
                    {
                        data: 'Email'
                    },
                    {
                        data: 'UserId',
                    }
                ]
                $('#tableUsers').DataTable({
                    responsive: true,
                    data: users,
                    pageLength: 15,
                    destroy: true,
                    //scrollX: true,
                    dom: "<'col-sm-12 col-md-6 pull-left'l><'col-sm-12 col-md-6 pull-right'f>rt<'col-sm-12 col-md-6 pull-left'i><'col-sm-12 col-md-6 pull-right'p>",
                    columns: columns,
                    language: {
                        processing: "Tratamiento en curso...",
                        search: "Buscar&nbsp;:",
                        lengthMenu: "Elementos por page _MENU_",
                        info: "",
                        infoEmpty: "",
                        infoFiltered: "(Filtrado con _MAX_ utilisateurs en total)",
                        infoPostFix: "",
                        loadingRecords: "Cargando...",
                        zeroRecords: "No hay elementos para mostrar",
                        emptyTable: "No hay datos disponibles en la tabla",
                        paginate: {
                            first: "Premier",
                            previous: "Previous",
                            next: "Suivant",
                            last: "Dernier"
                        },
                        aria: {
                            sortAscending: ": Activar para ordenar la columna en orden ascendente",
                            sortDescending: ": Activar para ordenar la columna en orden descendente"
                        }
                    },
                    drawCallback: function (settings) {
                        $(".loadUser").on("click", function () {
                            var id = $(this).data("id");
                            var user = users.find(u => u.Id === id);
                            that.LoadUser(user);
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
        commerce.amazon.web.users.Init();
    });
    // The rest of the code goes here!
}(window.jQuery, window, document));
