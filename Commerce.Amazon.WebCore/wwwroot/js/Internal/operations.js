var commerce = commerce || {};
commerce.amazon = commerce.amazon || {};
commerce.amazon.web = commerce.amazon.web || {};

//Declare a class, with parameteres
commerce.amazon.web.operations =
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
		Commerce.Amazon.Web.operations.Init();
	});
	// The rest of the code goes here!
}(window.jQuery, window, document));
