# range2prefix

Simple application show given a range start and a range end, we can generate the necessary prefixes between them.

Proof of concepts, under the assumptions:
1. Range inputs are already sanitized, whole positive integers only. So assumed the phone numbers already cleanse of plus signs, spaces, and brackets.
2. Range end is always bigger than range start
3. I use int32 here... so maybe too small/short for certain phone numbers, but like I said, just a proof of concepts.
4. If the length of numbers are not the same, I'm padding the range start with zeros. Not sure this is the correct move, but have problems if the numbers length are different.
5. First number should not be zero since I'm treating the number as integer, any numbers that start with zero will have the zero ignored - I think there should be no use case for telephone numbers to start with zero?


Some examples to test with:
1415900100 - 1415900109
1415900100 - 1415900108
1415900100 - 1415900199
1415900100 - 1415900198
1415900100 - 1415900999
1415900156 - 1415900978




