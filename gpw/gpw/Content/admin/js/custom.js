function main() {
    (function () {
        'use strict';
        $(document).ready(function () {

            /*
	          =======================================================================
		  		         Chosen Script Script
	          =======================================================================
	        */
            var config = {
                '.chosen-select': {
                    no_results_text: "Không tìm thấy kết quả nào.",
                    width: '100%',
                    allow_single_deselect: true
                },
                '.chosen_select_width200': { no_results_text: "Không tìm thấy kết quả!", width: '200px', allow_single_deselect: true },
                '.chosen-select-deselect': { allow_single_deselect: true, no_results_text: "Không tìm thấy kết quả!" },
                '.chosen-select-no-single': { disable_search_threshold: 10 },
                '.chosen_select_no_results': { no_results_text: 'Không tìm thấy kết quả!', width: 'auto', allow_single_deselect: true },
                '.chosen-select-width': { width: "95%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }




        })

    }());
}
main();