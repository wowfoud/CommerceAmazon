var commerce = commerce || {};
commerce.amazon = commerce.amazon || {};
commerce.amazon.web = commerce.amazon.web || {};

//Declare a class, with parameteres
commerce.amazon.web.user =
    (function () {
        var MyAuxClass = function () {

            var that = this;

            this.Init = function () {
                that.InitDatePicker();
            };

            //----------------------Init------------------------//


            this.InitBuyProduct = function () {
                that.RunViewDetailsPostUser();
            }
            this.InitPostsToBuy = function () {

                $('#btnFilterPostsToBuy').click(function () {
                    var dateMin = $('#dateMin').val();
                    var dateMax = $('#dateMax').val();
                    var idGroup = $('#sltGroup').val();
                    var state = 2;
                    that.ViewPostsToBuy(dateMin, idGroup, state, function (data) {
                        that.LoadPostsToBuy(data);
                    });
                })

                that.FindMyGroups(function (groups) {
                    that.LoadMyGroups(groups);
                    $('#btnFilterPostsToBuy').click();
                });
            }
            this.InitPostsPurchased = function () {

                $('#btnFilterPostsPurchased').click(function () {
                    var dateMin = $('#dateMin').val();
                    var dateMax = $('#dateMax').val();
                    var idGroup = $('#sltGroup').val();
                    var state = 3;
                    that.ViewPostsToBuy(dateMin, idGroup, state, function (data) {
                        that.LoadPostsPurchased(data);
                    });
                })

                that.FindMyGroups(function (groups) {
                    that.LoadMyGroups(groups);
                    $('#btnFilterPostsPurchased').click();
                });
            }
            this.InitPostsExpired = function () {

                $('#btnFilterPostsExpired').click(function () {
                    var dateMin = $('#dateMin').val();
                    var dateMax = $('#dateMax').val();
                    var idGroup = $('#sltGroup').val();
                    var state = 4;
                    that.ViewPostsToBuy(dateMin, idGroup, state, function (data) {
                        that.LoadPostsExpired(data);
                    });
                })

                that.FindMyGroups(function (groups) {
                    that.LoadMyGroups(groups);
                    $('#btnFilterPostsExpired').click();
                });
            }
            this.InitPostsUser = function () {

                $('#btnFilterPostsUser').click(function () {
                    var filter = {
                        DateDebut: $('#dateMin').val(),
                        DateFin: $('#dateMax').val(),
                        IdGroup: $('#sltGroup').val(),
                        StatePlan: 1
                    };
                    that.ViewPostsUser(filter, function (data) {
                        that.LoadPostsUser(data);
                    });
                })

                that.FindMyGroups(function (groups) {
                    that.LoadMyGroups(groups);
                    $('#btnFilterPostsUser').click();
                });
            }
            this.InitNewPost = function () {
                that.FindMyGroups(function (groups) {
                    that.LoadMyGroups(groups);
                    $('#sltGroup').change(function () {
                        var id = $(this).val();
                        var g = groups.find(e => e.Id == id);
                        $('#CountUsers').val(g.CountUsers);
                    });
                    $('#sltGroup').change();
                });
                $('#idBtnSavePost').click(function () {
                    $('#errorMsgDiv').text('');
                    var post = {
                        GroupId: $('#sltGroup').val(),
                        Url: $('#Url').val(),
                        Description: $('#Description').val(),
                        Prix: $('#Prix').val()
                    };
                    if (!post.GroupId) {
                        $('#errorMsgDiv').text('SVP, Selectioner un groupe.');
                        return;
                    }
                    if (!post.Url) {
                        $('#errorMsgDiv').text('url est obligatoire.');
                        return;
                    }
                    if (!post.Prix) {
                        $('#errorMsgDiv').text('Prix est obligatoire.');
                        return;
                    }
                    Swal.showLoading();
                    that.PostProduit(post, function (result) {
                        if (!!result && result.Status === 0) {
                            //alert(`Status: ${result.Status}, Message: ${result.Message}`);
                            Swal.fire({ title: 'Post enregistré avec succes', html: '', type: "success", confirmButtonColor: '#DB9D0A' });
                            $('#Url').val('');
                            $('#Description').val('');
                            $('#Prix').val('');
                        } else {
                            var msg = '';
                            if (!result || !result.Message) {
                                msg = 'un erreur non controllé s\'est produit.';
                            } else {
                                msg = result.Message;
                            }
                            $('#errorMsgDiv').text(msg);
                        }
                    });
                })

            }
            this.InitDetailPost = function () {
                that.RunViewDetailsPostUser();
            }

            //----------------------end Init------------------------//



            //----------------------Function------------------------//

            this.InitDatePicker = function () {
                $('.date, .dateTo, .dateFrom, .dateToday').datetimepicker({
                    format: "DD/MM/YYYY",
                    showTodayButton: true,
                    showClear: true,
                    showClose: true,
                    locale: "es",
                    viewMode: 'days',
                    widgetParent: 'body'
                });
                //$('.dateToday').data("DateTimePicker").maxDate(moment(Date.now()));
                $('.dateToday').each(function (i, ele) {
                    $(ele).data("DateTimePicker").maxDate(moment(Date.now()));
                })
                $('.date, .dateTo, .dateFrom, .dateToday').each(function (i, ele) {
                    $(ele).data("DateTimePicker").minDate(moment('01/01/1753'));
                })
                $(".dateFrom").on("dp.change", function (e) {
                    if ($('.dateTo').val()) {
                        $('.dateTo').data("DateTimePicker").minDate(e.date);
                    } else {
                        $(".dateFrom").data("DateTimePicker").maxDate(moment(Date.now()));
                    }
                });
                $(".dateTo").on("dp.change", function (e) {
                    $('.dateFrom').data("DateTimePicker").maxDate(e.date);
                });
                $('.date, .dateTo, .dateFrom, .dateToday').on('dp.show', function () {
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


            //----------------------events------------------------//

            $('#idBtnComment').click(function () {
                var idPost = $('#idPost').val();
                var comment = $('#idComment').val();
                if (!!that.SelectedScreen && !!idPost) {
                    that.UploadScreen(that.SelectedScreen, function (filename) {
                        that.CommentPost(filename, comment, idPost, function (result) {
                            if (!!result && result.Status === 0) {
                                Swal.fire({ title: 'Votre commentaire enregistré avec succes', html: '', type: "success", confirmButtonColor: '#DB9D0A' });
                            } else {

                            }
                        });
                    });
                }
            })

            $('#screen-comment').change(function () {
                that.SelectedScreen = this.files[0];
                var fr = new FileReader();
                fr.onload = function () {
                    //console.log(fr.result);
                    $('#idImgScreen').attr('src', fr.result);
                }
                fr.readAsDataURL(that.SelectedScreen);
            });

            this.LoadPostsUser = function (posts) {
                var columns = [
                    {
                        data: 'Id'
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
                        data: 'CountNotified',
                    },
                    {
                        data: 'CountCommented',
                    },
                    {
                        data: 'CountExpired',
                    },
                    {
                        data: 'Total',
                    }
                ]
                $('#tablePostsUser').DataTable({
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

            this.LoadPostsToBuy = function (posts) {
                var columns = [
                    {
                        data: 'Id'
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
                        data: 'DateNotified'
                    },
                    {
                        data: 'DateLimite'
                    },
                    {
                        data: '',
                        render: function (data, type, row) {
                            return `<a href='/Post/BuyProduct?idPost=${row.Id}' style='cursor: pointer; text-decoration: underline' class='cursorPointer' data-id='${row.Id}'>Acheter</a>`;
                        }
                    }
                ]
                $('#tablePostsToBuy').DataTable({
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

                    },
                    createdRow: function (row, data, index) {
                        var title = '';
                        if (data.DaysRemaining === 1) {
                            $(row).addClass('danger');
                            title = `un jour restante pour acheter ce produit`;
                        }
                        if (data.DaysRemaining > 1) {
                            title = `${data.DaysRemaining} jours restantes pour acheter ce produit`;
                        }
                        $(row).find('td:eq(5)')
                            .attr('title', title);
                    }
                });
                $($.fn.dataTable.tables()).DataTable().columns.adjust();
            }

            this.LoadPostsPurchased = function (posts) {
                var columns = [
                    {
                        data: 'Id'
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
                        data: '',
                        render: function (data, type, row) {
                            return `<a href='/Post/DetailPost?idPost=${row.Id}' style='cursor: pointer; text-decoration: underline' class='cursorPointer' data-id='${row.Id}'>Detail</a>`;
                        }
                    }
                ]
                $('#tablePostsPurchased').DataTable({
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

            this.LoadPostsExpired = function (posts) {
                var columns = [
                    {
                        data: 'Id'
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
                        data: '',
                        render: function (data, type, row) {
                            return `<a href='/Post/BuyProduct?idPost=${row.Id}' style='cursor: pointer; text-decoration: underline' class='cursorPointer' data-id='${row.Id}'>Detail</a>`;
                        }
                    }
                ]
                $('#tablePostsExpired').DataTable({
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

            this.RunViewPost = function () {
                var idPost = that.GetParameter('idPost');
                if (!!idPost) {
                    that.ViewPost(idPost, function (post) {
                        that.LoadPost(post);
                    });
                }
            }

            this.RunViewDetailsPostUser = function () {
                var idPost = that.GetParameter('idPost');
                if (!!idPost) {
                    that.ViewDetailsPostUser(idPost, function (post) {
                        that.LoadDetailsPostUser(post);
                        //var url = `/Post/DownloadScreenComment?idPost=${idPost}&idUser=${post.IdUser}`;
                        //var url = `/Post/GetPathComment?idPost=${idPost}&idUser=${post.IdUser}`;
                        //$('#idImgScreen').attr('src', url);
                        //that.DownloadImage(url);
                        //var link = document.createElement('a');
                        //link.addEventListener('click', function (ev) {
                        //    link.href = url;
                        //    link.download = "image.jpg";
                        //}, false);
                        //link.click();
                    });
                }
            }

            this.GetParameter = function (name) {
                var params = new URLSearchParams(document.location.search);
                return params.get(name);
            }

            this.LoadPost = function (post) {
                $('#idPost').val(post.Id);
                $('#Url').val(post.Url);
                $('#Url').text(post.Url);
                $('#Url').attr('href', post.Url);
                $('#Description').val(post.Description);
                $('#Prix').val(post.Prix);
                $('#DateCreated').val(post.DateCreated);
                $('#DateNotified').val(post.DateNotified);
                $('#DateLimite').val(post.DateLimite);
            }
            
            this.LoadDetailsPostUser = function (post) {
                $('#idPost').val(post.Id);
                //$('#Url').val(post.Url);
                $('#Url').text(post.Url);
                $('#Url').attr('href', post.Url);
                $('#Description').val(post.Description);
                $('#Prix').val(post.Prix);
                $('#DateCreated').val(post.DateCreated);
                $('#DateNotified').val(post.DateNotified);
                $('#DateLimite').val(post.DateLimite);
                $('#idComment').val(post.Comment);
                $('#DateComment').val(post.DateComment);
            }

            //----------------------End event------------------------//
            //----------------------AJAX------------------------//
            this.PostProduit = function (post, handler) {
                $.ajax({
                    type: "POST",
                    url: "/Post/PostProduit",
                    data: post,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            console.log(data);
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
            this.FindMyGroups = function (handler) {
                $.ajax({
                    type: "POST",
                    url: "/Post/FindMyGroups",
                    success: function (data) {
                        if (!!data) {
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
            this.LoadMyGroups = function (groups) {
                $('#sltGroup').empty();
                if (groups.length > 1) {
                    $('#sltGroup').append(
                        $('<option></option>').val("").text("Sélectionner")
                    );
                }
                $.each(groups, function (i, group) {
                    $('#sltGroup').append(
                        $('<option></option>').val(group.Id).text(group.Name)
                    )
                });
            }
            this.CanEditPost = function (idPost, handler) {
                var data = {
                    IdPost: idPost,
                };
                $.ajax({
                    type: "POST",
                    url: "/Post/CanEditPost",
                    data: data,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            console.log(data);
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
            this.PlanifierNotificationPost = function (idPost, handler) {
                var data = {
                    IdPost: idPost,
                };
                $.ajax({
                    type: "POST",
                    url: "/Admin/PlanifierNotificationPost",
                    data: data,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            console.log(data);
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
            this.ViewPost = function (idPost, handler) {
                var data = {
                    IdPost: idPost,
                };
                $.ajax({
                    type: "POST",
                    url: "/Post/ViewPost",
                    data: data,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            console.log(data);
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
            this.ViewDetailsPostUser = function (idPost, handler) {
                var data = {
                    IdPost: idPost,
                };
                $.ajax({
                    type: "POST",
                    url: "/Post/ViewDetailsPostUser",
                    data: data,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            console.log(data);
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
            this.ViewPostsUser = function (filter, handler) {
                
                $.ajax({
                    type: "POST",
                    url: "/Post/ViewPostsUser",
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
            this.ViewPostsToBuy = function (date = undefined, idGroup = 1, state = undefined, handler) {
                var data = {
                    Date: date,
                    IdGroup: idGroup,
                    StatePlan: state
                };
                $.ajax({
                    type: "POST",
                    url: "/Post/ViewPostsToBuy",
                    data: data,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            console.log(data);
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
            this.CommentPost = function (filename, comment, idPost, handler) {
                var data = {
                    Comment: comment,
                    ScreenComment: filename,
                    IdPost: idPost
                };
                $.ajax({
                    type: "POST",
                    url: "/Post/CommentPost",
                    data: data,
                    success: function (data) {
                        if (HandleResponse(data)) {
                            console.log(data);
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
            this.UploadScreen = function (selectedFile, handler) {
                if (!!selectedFile) {
                    if (window.FormData !== undefined) {
                        var fileData = new FormData();
                        fileData.append('listFiles', selectedFile, selectedFile.name);
                        $.ajax({
                            url: '/Post/UploadScreen',
                            type: "POST",
                            dataType: 'json',
                            contentType: false,
                            processData: false,
                            data: fileData,
                            success: function (filename) {
                                if (HandleResponse(filename)) {
                                    handler(filename);
                                } else {
                                    that.OnError();
                                }
                            },
                            error: function (err) {
                                console.log(err.statusText);
                            }
                        });
                    } else {
                        console.log("FormData is not supported.");
                    }
                }
            }
            this.DownloadImage = function (url, filename, error) {
                let xmlhttp = new XMLHttpRequest();
                xmlhttp.open("POST", url, true);
                xmlhttp.setRequestHeader('Content-Type', 'image/jpg');
                //xmlhttp.setRequestHeader('Cache-Control', 'no-cache');
                xmlhttp.responseType = "blob";
                xmlhttp.onload = function (e) {
                    if (this.status == 200) {
                        if (this.response.size === 8) {
                            AlertError(error);
                        } else {
                            const blob = this.response;
                            const blobUrl = window.URL.createObjectURL(blob);
                            console.log(blobUrl);
                            $('#idImgScreen').attr('src', blob);
                        }
                    }
                };
                xmlhttp.send();
            }
            this.OnError = function () {
                console.log("Error find data");
            }

            //----------------------End AJAX------------------------//

        };
        return new MyAuxClass();
    })();

// IIFE - Immediately Invoked Function Expression
(function ($, window, document) {

    // The $ is now locally scoped
    // Listen for the jQuery ready event on the document
    $(function () {
        //Document Ready Actions
        commerce.amazon.web.user.Init();
    });
    // The rest of the code goes here!
}(window.jQuery, window, document));
