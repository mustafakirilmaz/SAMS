import { ResultType } from '../enums/result-type';

class ServiceResult<T> {
  public messages: string[];
  public resultType: ResultType | string;
  public data: T;
}

export default ServiceResult;
