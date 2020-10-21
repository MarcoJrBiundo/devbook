import { Interest } from './Interest';
import { Photo } from './Photo';
import { Skill } from './Skill';

export interface User {

    id: number;
    username: string;
    firstName: string;
    lastName: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    githubLink: string;
    city: string;
    country: string;
    status?: string;
    rating?: number;
    photos?: Photo[];
    interest: string;
    skills?: Skill[];

}
