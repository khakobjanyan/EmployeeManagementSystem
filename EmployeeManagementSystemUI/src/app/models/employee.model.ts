import {ClassifierModel} from "./classifier.model";

export class Employee{
    id: number;
    firstName: string;
    lastName:string;
    position: string;
    startDate: Date;
    status: ClassifierModel;
    profilePictureUrl?: string ;
}

