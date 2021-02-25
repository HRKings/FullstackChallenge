import { NameData } from './NameData';

export interface PagedData {
	totalItems: number;
	pageSize: number;
	value: NameData[];
}