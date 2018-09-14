declare var moment:any;
import { NgbDateStruct, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { environment } from '../../../environments/environment';
export class NgbDateMomentParserFormatter extends NgbDateParserFormatter {
  parse(value: string): NgbDateStruct {
    if (value) {

      var parsed = moment(value, environment.localization.dateFormat);
      if (parsed.isValid()) {

        var date = parsed;        //toDate()

        return {
          year: date.year(),
          month: date.month() + 1,
          day: date.date()
        };

      }
    }
    return null;
  }

  format(date: NgbDateStruct): string {
    if (date == null) return null;

    var parsed = moment(new Date(date.year, date.month - 1, date.day));

    return parsed.format(environment.localization.dateFormat);
  }
}