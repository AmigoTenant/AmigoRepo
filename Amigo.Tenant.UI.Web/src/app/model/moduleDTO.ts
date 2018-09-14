import  {ActionDtoModule} from './actiondto';
 export class ModuleDto {

     public code: string;
     public name: string;
     public url: string;
     public parentModuleCode: string;
     public sortOrder: number;
     public onlyParents: boolean;
     public actions : ActionDtoModule[]; 


 }