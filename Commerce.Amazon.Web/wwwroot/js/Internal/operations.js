var commerce = commerce || {};
commerce.amazon = commerce.amazon || {};
commerce.amazon.web = commerce.amazon.web || {};

//Declare a class, with parameteres
commerce.amazon.web.operation =
    (function () {
        var MyAuxClass = function () {

            var that = this;

            this.Init = function () {
                that.InitDatePicker();
            };

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

            $('#btnFilter').click(function () {

            })
            
            $('#idBtnComment').click(function () {
                var idPost = $('#idPost').val();
                var comment = $('#idComment').val();
                if (!!that.SelectedScreen && !!idPost) {
                    that.UploadScreen(that.SelectedScreen, function (filename) {
                        that.CommentPost(filename, comment, idPost, function (result) {
                            alert(`Status: ${result.Status}, Message: ${result.Message}`);
                        });
                    });
                }
            })
            
            $('#idBtnSavePost').click(function () {
                $('#errorMsgDiv').text('');
                var url = $('#Url').val();
                var description = $('#Description').val();
                var prix = $('#Prix').val();
                that.PostProduit(url, description, prix, function (result) {
                    $('#errorMsgDiv').text(result.Message);
                    alert(`Status: ${result.Status}, Message: ${result.Message}`);
                });
            })
            
            $('#btnFilterPostsUser').click(function () {
                var dateMin = $('#dateMin').val();
                var dateMax = $('#dateMax').val();
                var idGroup = $('#sltGroup').val();
                var state = 1;
                that.ViewPostsUser(dateMin, idGroup, state, function (data) {
                    that.LoadPostsUser(data);
                });
            })
            
            $('#btnFilterPostsToBuy').click(function () {
                var dateMin = $('#dateMin').val();
                var dateMax = $('#dateMax').val();
                var idGroup = $('#sltGroup').val();
                var state = 1;
                that.ViewPostsToBuy(dateMin, idGroup, state, function (data) {
                    that.LoadPostsToBuy(data);
                });
            })
            $('#screen-comment').change(function () {

                that.SelectedScreen = this.files[0];

                var fr = new FileReader();
                fr.onload = function () {
                    console.log(fr.result);
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
                        data: 'Url'
                    },
                    {
                        data: 'Description'
                    },
                    {
                        data: 'Prix'
                    },
                    {
                        data: 'DateCreate',
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
                        data: 'Url'
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
                        
                    }
                });

                $($.fn.dataTable.tables()).DataTable().columns.adjust();
            }

            this.DetailPost = function () {
                var params = new URLSearchParams(document.location.search);
                var idPost = params.get('idPost');
                that.ViewPost(idPost, function (post) {
                    that.LoadPost(post);
                });
            }

            this.LoadPost = function (post) {
                $('#idPost').val(post.Id);
                $('#Url').val(post.Url);
                $('#Description').val(post.Description);
                $('#Prix').val(post.Prix);
                $('#Date').val(post.Date);
            }

            //----------------------End event------------------------//
            //----------------------AJAX------------------------//
            this.PostProduit = function (url, description, prix, handler) {
                var post = {
                    Url: url,
                    Description: description,
                    Prix: prix
                };
                $.ajax({
                    type: "POST",
                    url: "/Admin/PostProduit",
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
            this.CanEditPost = function (idPost, handler) {
                var data = {
                    IdPost: idPost,
                };
                $.ajax({
                    type: "POST",
                    url: "/Admin/CanEditPost",
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
            this.ViewPlaningPost = function (idPost, handler) {
                var data = {
                    IdPost: idPost,
                };
                $.ajax({
                    type: "POST",
                    url: "/Admin/ViewPlaningPost",
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
            this.NotifyUsers = function (idPost, users, handler) {
                var data = {
                    IdPost: idPost,
                    Users: users
                };
                $.ajax({
                    type: "POST",
                    url: "/Admin/NotifyUsers",
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
            this.ViewPostsUser = function (date = undefined, idGroup = 1, state = undefined, handler) {
                var data = {
                    Date: date,
                    IdGroup: idGroup,
                    StatePlan: state
                };
                $.ajax({
                    type: "POST",
                    url: "/Admin/ViewPostsUser",
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
            this.ViewPostsToBuy = function (date = undefined, idGroup = 1, state = undefined, handler) {
                var data = {
                    Date: date,
                    IdGroup: idGroup,
                    StatePlan: state
                };
                $.ajax({
                    type: "POST",
                    url: "/Admin/ViewPostsToBuy",
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
                    url: "/Admin/CommentPost",
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
                            url: '/Admin/UploadScreen',
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
            this.OnError = function () {
                console.log("Error find data");
            }

            //----------------------End AJAX------------------------//

            this.InitBuyProduct = function () {

            }
            this.InitPostsToBuy = function () {

            }
            this.InitPostsUser = function () {

            }
        };
        return new MyAuxClass();
    })();


// IIFE - Immediately Invoked Function Expression
(function ($, window, document) {

    // The $ is now locally scoped
    // Listen for the jQuery ready event on the document
    $(function () {
        //Document Ready Actions
        commerce.amazon.web.operation.Init();
    });
    // The rest of the code goes here!
}(window.jQuery, window, document));
