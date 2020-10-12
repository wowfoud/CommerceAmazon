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
                    url: "/Admin/ViewPost",
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
            this.CommentPost = function (comment, idPost, handler) {
                var data = {
                    ScreenComment: comment,
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
                            success: function (data) {
                                if (HandleResponse(data)) {
                                    handler(data);
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
