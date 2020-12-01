
var commerce = commerce || {};
commerce.amazon = commerce.amazon || {};
commerce.amazon.web = commerce.amazon.web || {};

//Declare a class, with parameteres
commerce.amazon.web.admin =
    (function () {
        var MyAuxClass = function () {
            var that = this;


            this.Init = function () {
                that.InitDatePicker();
            };

            this.InitUsers = function () {

                $('#idBtnFilter').click(function () {
                    var filter = {
                        GroupId: $('#sltGroup').val()
                    };
                    that.FindUsers(filter, function (users) {
                        that.LoadUsers(users);
                    });
                });
                $('#idOpenModalUser').click(function () {
                    that.LoadUser({}, false)
                    //$('#myModalUsuario').modal('show');
                });
                $('#idBtnSaveUser').click(function () {
                    var user = that.ValidUser();
                    if (!!user) {
                        that.SaveUser(user, function (data) {
                            if (data && data.Status === 0) {
                                $('.modal button.myclose').click();
                                that.User = user;
                                var filter = {
                                    GroupId: $('#sltGroup').val()
                                };
                                that.FindUsers(filter, function (users) {
                                    that.LoadUsers(users);
                                });
                            } else {
                                $("#errorMsgDiv").html('<span class="error">' + data.Message + '</span>')
                            }
                        });
                    }
                });

                that.FindGroups(undefined, function (groups) {
                    $('#groupes').empty();
                    $('#sltGroup').empty();
                    if (groups.length > 1) {
                        $('#sltGroup').append(
                            $('<option></option>').val("").text("Sélectionner")
                        );
                    }
                    for (var i = 0; i < groups.length; i++) {
                        var group = groups[i];
                        $('#groupes').append(
                            $('<option></option>').val(group.Id).text(group.Name)
                        )
                        $('#sltGroup').append(
                            $('<option></option>').val(group.Id).text(group.Name)
                        )
                    }
                })
                $('#idBtnFilter').click();
            };

            this.InitGroups = function () {
                $('#idBtnSaveGroup').click(function () {
                    that.SaveGroup();
                });
                that.FindGroups(undefined, function (groups) {
                    that.LoadGroups(groups);
                });
            }

            this.InitPostsToSend = function () {
                that.FindGroups(undefined, function (groups) {
                    $('#sltGroup').empty();
                    if (groups.length > 1) {
                        $('#sltGroup').append(
                            $('<option></option>').val("").text("Sélectionner")
                        );
                    }
                    for (var i = 0; i < groups.length; i++) {
                        var group = groups[i];
                        $('#sltGroup').append(
                            $('<option></option>').val(group.Id).text(group.Name)
                        )
                    }
                })
                $('#btnFilterPostsToSend').click(function () {
                    var filter = {
                        DateDebut: $('#dateMin').val(),
                        DateFin: $('#dateMax').val(),
                        IdGroup: $('#sltGroup').val(),
                        StatePlan: 1
                    };
                    that.FindPostsToSend(filter, function (data) {
                        that.LoadPostsToSend(data);
                    });
                });
                $('#btnFilterPostsToSend').click();
            }

            this.InitHistoriques = function () {
                
            }
            
            //----------------------AJAX------------------------//

            this.ValidUser = function () {
                $("#errorMsgDiv").html('');
                var form = document.getElementById("formUsuario");
                var user = undefined;
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

                    var groupesId = [];
                    var options = document.getElementById('groupes').options;
                    for (var i = 0; i < options.length; i++) {
                        var o = options[i];
                        if (o.selected == true) {
                            groupesId.push(o.value);
                        }
                    }
                    user = {
                        Id: idUser,
                        Nom: $('#Nom').val(),
                        Prenom: $('#Prenom').val(),
                        Email: $('#Email').val(),
                        UserId: $('#UserId').val(),
                        Role: idRole,
                        GroupId: $('#GroupId').val(),
                        Groupes: groupesId
                    };
                    if (!idRole) {
                        $("#errorMsgDiv").html("SVP, Selectioner le role d'utilisateur");
                        user = undefined;
                    } else if (!user.UserId) {
                        $("#errorMsgDiv").html("UserId est obligatoire.");
                        user = undefined;
                    } else if (!user.Email) {
                        $("#errorMsgDiv").html("Email est obligatoire.");
                        user = undefined;
                    }
                }
                return user;
            }

            this.SaveUser = function (user, handler) {
                $.ajax({
                    type: "POST",
                    url: "/User/SaveUser",
                    data: user,
                    success: function (data) {
                        //console.log(data);
                        if (HandleResponse(data)) {
                            handler(data);
                        } else {
                            that.OnError();
                        }
                    },
                    error: function (err) {
                        that.OnError();
                    }
                });
            }

            this.SaveGroup = function () {
                $("#errorMsgDiv").html('');
                var form = document.getElementById("formGroup");
                $.validator.unobtrusive.parse(form)
                if ($(form).valid()) {
                    var group = {
                        Id: $('#Id').val(),
                        Name: $('#Name').val(),
                        MaxDays: $('#MaxDays').val(),
                        CountNotifyPerDay: $('#CountNotifyPerDay').val(),
                        CountUsersCanNotify: $('#CountUsersCanNotify').val(),
                        State: document.getElementById('State').checked ? 1 : 2
                    };
                    $.ajax({
                        type: "POST",
                        url: "/User/SaveGroup",
                        data: group,
                        success: function (data) {
                            //console.log(data);
                            if (HandleResponse(data)) {
                                if (data && data.Status === 0) {
                                    AlertSuccess('le groupe enregistré avec succès');
                                    that.FindGroups(undefined, function (groups) {
                                        that.LoadGroups(groups);
                                    });
                                } else {
                                    $("#errorMsgDiv").html('<span class="error">' + data.Message + '</span>')
                                    AlertError(data.Message);
                                }
                            } else {
                                that.OnError();
                            }
                        },
                        error: function (err) {
                            that.OnError();
                        }
                    });
                }
            }

            this.FindUsers = function (filter, handleUsers) {
                $.ajax({
                    type: "POST",
                    url: "/User/FindUsers",
                    data: filter,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            //that.LoadUsers(data);
                            handleUsers(data);
                        } else {
                            that.OnError();
                        }
                    },
                    error: function (err) {
                        that.OnError();
                    }
                })
            }

            this.FindGroups = function (stateGroup, handleLoadGroups) {
                var filter = {
                    StateGroup: stateGroup
                };
                $.ajax({
                    type: "POST",
                    url: "/User/FindGroups",
                    data: filter,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            handleLoadGroups(data);
                        } else {
                            that.OnError();
                        }
                    },
                    error: function (err) {
                        that.OnError();
                    }
                })
            }

            this.FindPostsToSend = function (filter, handler) {
                $.ajax({
                    type: "POST",
                    url: "/Admin/FindPostsToSend",
                    data: filter,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            handler(data);
                        } else {
                            that.OnError();
                        }
                    },
                    error: function (err) {
                        that.OnError();
                    }
                });
            }

            this.FindHistorique = function (filter, handler) {
                $.ajax({
                    type: "POST",
                    url: "/Admin/FindHistorique",
                    data: filter,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            handler(data);
                        } else {
                            that.OnError();
                        }
                    },
                    error: function (err) {
                        that.OnError();
                    }
                });
            }

            //----------------------end AJAX------------------------//
            //----------------------Function------------------------//


            this.OnError = function (error) {
                console.log("Error find data");
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
                var groupesId = !!user.Groupes ? user.Groupes : [];
                var options = document.getElementById('groupes').options;
                for (var i = 0; i < options.length; i++) {
                    var o = options[i];
                    if (groupesId.findIndex(g => g == o.value) > -1) {
                        o.selected = true;
                    } else {
                        o.selected = false;
                    }
                }
                if (user.Role == undefined) {
                    user.Role = 2;
                }
                var radios = document.getElementsByName('roleUser');
                for (var i = 0; i < radios.length; i++) {
                    if (radios[i].value == user.Role) {
                        radios[i].checked = true;
                    } else {
                        radios[i].checked = false;
                    }
                }
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
                    },
                    {
                        data: 'RoleName',
                    },
                    {
                        data: 'CountGroupes',
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
                    buttons: [{
                        text: 'Exportar excel',
                        extend: 'excelHtml5',
                        title: 'Utilisateurs',
                        className: 'btn btn-exportar'
                    },
                    {
                        text: 'Filtrer',
                        title: 'Filtrer les utilisateurs',
                        className: 'btn btn-exportar',
                        action: function (e, dt, node, config) {
                            
                        }
                    }],
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

            this.LoadGroups = function (groups) {
                var columns = [
                    {
                        data: 'Id',
                        render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        data: 'Name',

                    },
                    {
                        data: 'CountUsers'
                    },
                    {
                        data: 'MaxDays'
                    },
                    {
                        data: 'CountNotifyPerDay',
                    },
                    {
                        data: 'CountUsersCanNotify',
                    },
                    {
                        data: 'State',
                        render: function (data, type, row) {
                            if (data === 1) {
                                return `<input type="checkbox" class="form-control input-sm" checked disabled />`;
                            } else {
                                return `<input type="checkbox" class="form-control input-sm" disabled />`;
                            }
                        }
                    }
                ]
                that.tableGroups = $('#tableGroupes').DataTable({
                    responsive: true,
                    data: groups,
                    pageLength: 15,
                    destroy: true,
                    //scrollX: true,
                    select: true,
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

                    }
                });

                $($.fn.dataTable.tables()).DataTable().columns.adjust();
                that.tableGroups.off('select').on('select', function (e, dt, type, indexes) {
                    if (indexes.length > 0) {
                        var group = groups[indexes[0]];
                        if (!!group) {
                            if (type === 'row') {
                                that.SelectGroup(group);
                            }
                        }
                    }
                });
                that.tableGroups.off('deselect').on('deselect', function (e, dt, type, indexes) {
                    if (indexes.length > 0) {
                        that.ClearGroup();
                    }
                });
            }

            this.SelectGroup = function (group) {
                if (!!group) {
                    $('#Id').val(group.Id);
                    $('#Name').val(group.Name);
                    $('#MaxDays').val(group.MaxDays);
                    $('#CountNotifyPerDay').val(group.CountNotifyPerDay);
                    $('#CountUsersCanNotify').val(group.CountUsersCanNotify);
                    document.getElementById('State').checked = group.State === 1;
                } else {
                    $('#Id').val('');
                    $('#Name').val('');
                    $('#MaxDays').val('');
                    $('#CountNotifyPerDay').val('');
                    $('#CountUsersCanNotify').val('');
                }

            }

            this.LoadPostsToSend = function (posts) {
                var columns = [
                    {
                        data: 'Id'
                    },
                    {
                        data: 'Nom',
                        render: function (data, type, row) {
                            var user = '';
                            if (!!row.Nom) {
                                user += `${row.Nom} `;
                            }
                            if (!!row.Prenom) {
                                user += `${row.Prenom}`;
                            }
                            return user;
                        }
                    },
                    {
                        data: 'Url',
                        render: function (data, type, row) {
                            var url = data;
                            if (!!data && data.length > 70) {
                                url = data.substring(0, 67) + "...";
                            }
                            return `<a href="${data}" target="_blank">${url}</a>`;
                        }
                    },
                    {
                        data: 'Description'
                    },
                    {
                        data: 'Prix'
                    },
                    {
                        data: 'DateCreated',
                    },
                    {
                        data: 'DatePlanifie',
                    },
                    {
                        data: 'Groupe',
                    },
                    {
                        data: 'Total',
                        visible: false
                    },
                    {
                        data: '',
                        render: function (data, type, row) {
                            return `<a href="${data}">${url}</a>`;
                        }
                    }
                ]
                $('#tablePostsToSend').DataTable({
                    responsive: true,
                    data: posts,
                    pageLength: 15,
                    destroy: true,
                    //scrollX: true,
                    dom: "<'col-sm-12 col-md-6 pull-left'l><'col-sm-12 col-md-6 pull-right'f>rt<'col-sm-12 col-md-6 pull-left'i><'col-sm-12 col-md-6 pull-right'p>",
                    columns: columns,
                    language: {
                        processing: "Traitement en cours...",
                        search: "Chercher&nbsp;:",
                        lengthMenu: "éléments par page _MENU_",
                        info: "",
                        infoEmpty: "",
                        infoFiltered: "(Filtré avec _MAX_ postes au total)",
                        infoPostFix: "",
                        loadingRecords: "Chargement...",
                        zeroRecords: "Il n'y a pas d'éléments à afficher",
                        emptyTable: "aucune donnée disponible",
                        paginate: {
                            first: "Premier",
                            previous: "Previous",
                            next: "Suivant",
                            last: "Dernier"
                        },
                        aria: {
                            sortAscending: ": Activer pour trier la colonne par ordre croissant",
                            sortDescending: ": Activer pour trier la colonne par ordre décroissant"
                        }
                    },
                    drawCallback: function (settings) {

                    }
                });

                $($.fn.dataTable.tables()).DataTable().columns.adjust();
            }

            this.ClearGroup = function () {
                that.SelectGroup(undefined);
            }
            
            this.InitDatePicker = function () {
                $('.date, .dateTo, .dateFrom, .maxToday').datetimepicker({
                    format: "DD/MM/YYYY",
                    showTodayButton: true,
                    showClear: true,
                    showClose: true,
                    locale: "es",
                    viewMode: 'days',
                    widgetParent: 'body'
                });
                //$('.maxToday').data("DateTimePicker").maxDate(moment(Date.now()));
                $('.maxToday').each(function (i, ele) {
                    //console.log('maxToday');
                    $(ele).data("DateTimePicker").maxDate(moment(Date.now()));
                })
                $('.date, .dateTo, .dateFrom, .maxToday').each(function (i, ele) {
                    $(ele).data("DateTimePicker").minDate(moment('01/01/1753'));
                })
                //$(".dateTo").on("dp.change", function (e) {
                //    console.log('dateTo');
                //    if (!!$('.dateTo').val() && !!e.date) {
                //        $('.dateFrom').data("DateTimePicker").maxDate(e.date);
                //    }
                //});
                $(".dateFrom").on("dp.change", function (e) {
                    //console.log('dateFrom');
                    if (!!$('.dateFrom').val() && !!e.date) {
                        $('.dateTo').data("DateTimePicker").minDate(e.date);
                    }
                });
                $('.date, .dateTo, .dateFrom, .maxToday').on('dp.show', function () {
                    var datepicker = $('body').find('.bootstrap-datetimepicker-widget:last');
                    datepicker.switchClass('top', 'bottom');
                    if (datepicker.hasClass('bottom')) {
                        var top = $(this).offset().top + this.offsetHeight;
                        var left = $(this).offset().left;
                        datepicker.css({
                            'top': top + 'px',
                            'bottom': 'auto',
                            'left': left + 'px'
                        });
                    }
                    $('#ui-datepicker-div').addClass('hidden');
                });
                $('#ui-datepicker-div').on('dp.show', function () {
                    $('#ui-datepicker-div').addClass('hidden');
                });
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
        commerce.amazon.web.admin.Init();
    });
    // The rest of the code goes here!
}(window.jQuery, window, document));
