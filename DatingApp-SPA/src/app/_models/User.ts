import { Photo } from './Photo';

export interface User {
    id: number;
    userName: string;
    knownAs: string;
    gender: string;
    age: number;
    created: Date;
    lastActive: Date;
    city: string;
    country: string;
    photoUrl: string;

    interest?: string;
    introduction?: string;
    lookingFor?: string;
    photos?: Photo[];
    roles?: string[];
}
