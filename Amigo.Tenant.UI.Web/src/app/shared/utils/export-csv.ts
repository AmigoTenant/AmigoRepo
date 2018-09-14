import { Component } from '@angular/core';
import { ExportReportStructure } from './export-report-structure';
declare var moment: any;
import { environment } from '../../../environments/environment';

export class ExportCsv {

    csvSeparator = ',';
    reportStructure: ExportReportStructure[];

    exportingCSV(filename: string, data:Object[]) {
        let csv = '';

        //headers
        for (let i = 0; i < this.reportStructure.length; i++) {
            if (this.reportStructure[i].header) {
                csv += this.reportStructure[i].header;

                if (i < (this.reportStructure.length - 1)) {
                    csv += this.csvSeparator;
                }
            }
        }

        //body        
        data.forEach((record, j) => {
            csv += '\n';
            for (let i = 0; i < this.reportStructure.length; i++) {
                if (this.reportStructure[i].field) {
                    var value = "";
                    if (record[this.reportStructure[i].field] != null) {
                        if (this.reportStructure[i].type == 'date') {
                            var dateParsed = moment(record[this.reportStructure[i].field]);
                            value = dateParsed.format(environment.localization.dateFormat);
                            csv += value;
                        }

                        if (this.reportStructure[i].type == 'time') {
                            var timeParsed = moment(record[this.reportStructure[i].field]);
                            value = timeParsed.format(environment.localization.timeFormat);
                            csv += value;
                        }

                        if (value == "")
                            csv += record[this.reportStructure[i].field];
                    } else {
                        csv += "";
                    }


                    if (i < (this.reportStructure.length - 1)) {
                        csv += this.csvSeparator;
                    }
                }
            }
        });
        this.downloadFile(csv, filename);
    }

    downloadFile(text, filename) {
        //debugger;
        var blob = new Blob([text], { type: 'text/csv;charset=utf-8;' });
        if (navigator.msSaveBlob) { // IE 10+
            navigator.msSaveBlob(blob, filename);
        }
        else {
            var link = document.createElement("a");
            if (link.download !== undefined) {
                // Browsers that support HTML5 download attribute
                var url = URL.createObjectURL(blob);
                link.setAttribute("href", url);
                link.setAttribute("download", filename);
                link.style.visibility = 'hidden';
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
            }
        }
    }

    downloadFile2(text, filename) {
        //debugger;

        // var file = new File;

        var link = document.createElement("a");
        if (link.download !== undefined) {
            // Browsers that support HTML5 download attribute
            link.setAttribute("href", 'C:\ExcelTemp\\test1.xlsx');
            link.setAttribute("download", filename);
            link.style.visibility = 'hidden';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
    }

}