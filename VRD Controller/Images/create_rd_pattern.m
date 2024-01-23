w = 48;
h = 48;

ndots = 30;

bw = zeros(h, w);

x = randi(w, [ndots 1]);
y = randi(h, [ndots 1]);

for k = 1:ndots
   for xi = max(1, x(k)-1):min(w, x(k)+1)
      bw(y(k), xi) = 1;
   end
   for yi = max(1, y(k)-1):min(h, y(k)+1)
      bw(yi, x(k)) = 1;
   end
end

imshow(bw);