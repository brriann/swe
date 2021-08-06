import Redis from 'ioredis';
import Strings from './strings';
import Hashes from './hashes';
import Lists from './Lists';
import Sets from './sets';
import SortedSets from './sortedSets';

// const redis = new Redis();

// redis.set('nameFromApp','bfostFromApp');
// redis.get('nameFromApp', (err, result) => {
//     console.log(result);
// });


const redis = new Redis(); // { password: 'man3294ehd'}

Strings(redis);
Hashes(redis);
Lists(redis);
Sets(redis);
SortedSets(redis);

// docker run --name bfostredis2 -p 127.0.0.1:6379:6379 redis